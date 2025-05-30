using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.BaseDto;
using DA.ZenPharma.Application.Dtos.OrderDto;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IProductService productService, ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productService = productService;
            _logger = logger;
        }
        public async Task UpdateAsync(OrderCreateDto dto)
        {
            if (!dto.Id.HasValue)
                throw new ArgumentException("Id đơn hàng không được để trống.");

            var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(dto.Id.Value);
            if (order == null)
                throw new ArgumentException($"Đơn hàng {dto.Id} không tồn tại.");

            if (dto.Details == null || !dto.Details.Any())
                throw new ArgumentException("Không có chi tiết đơn hàng.");

            // Cập nhật thuộc tính đơn hàng
            order.BranchId = dto.BranchId;
            order.CustomerName = string.IsNullOrEmpty(dto.CustomerName) ? "Khách lẻ" : dto.CustomerName;
            order.CustomerPhone = dto.CustomerPhone;
            order.TotalAmount = dto.TotalAmount;
            order.OrderStatus = dto.OrderStatus;
            order.UserId = dto.HandledById;
            order.UpdateDate = DateTime.UtcNow;

            // Xóa chi tiết đơn hàng cũ
            var existingDetails = order.OrderDetails.ToList();
            if (existingDetails.Any())
            {
                _unitOfWork.OrderDetails.RemoveRangeAsync(existingDetails);
                order.OrderDetails.Clear();
            }

            // Thêm chi tiết đơn hàng mới
            foreach (var detailDto in dto.Details)
            {
                // Kiểm tra sản phẩm
                var product = await _unitOfWork.Products.GetByIdAsync(detailDto.ProductId);
                if (product == null)
                    throw new ArgumentException($"Sản phẩm {detailDto.ProductId} không tồn tại.");

                // Kiểm tra lô hàng
                var batch = await _unitOfWork.InventoryBatches.GetByIdAsync(detailDto.InventoryBatchId);
                if (batch == null || batch.BranchId != dto.BranchId)
                    throw new ArgumentException($"Lô hàng {detailDto.InventoryBatchId} không hợp lệ hoặc không thuộc chi nhánh {dto.BranchId}.");

                // Kiểm tra đơn vị
                var units = await _productService.GetProductUnitsAsync(detailDto.ProductId);
                if (units == null || !units.Any())
                    throw new ArgumentException($"Sản phẩm {detailDto.ProductId} không có đơn vị được cấu hình.");

                var unit = units.FirstOrDefault(u => u.Unit == detailDto.Unit);
                if (unit == null)
                    throw new ArgumentException($"Đơn vị {detailDto.Unit} không hợp lệ cho sản phẩm {detailDto.ProductId}.");

                // Ánh xạ và thêm chi tiết đơn hàng
                var detail = _mapper.Map<OrderDetail>(detailDto);
                detail.Id = detailDto.Id ?? Guid.NewGuid(); // Sử dụng Id nếu có, nếu không tạo mới
                detail.OrderId = order.Id;
                detail.TotalAmount = detail.Quantity * detail.UnitPrice;
                order.OrderDetails.Add(detail);
            }

            // Cập nhật đơn hàng
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }


        public async Task<OrderDto?> GetByIdAsync(Guid id)
        {
            var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(id);
            return order == null ? null : _mapper.Map<OrderDto>(order);
        }

        public async Task AddAsync(OrderCreateDto dto)
        {
            foreach (var detail in dto.Details)
            {
                // Lấy lô hàng
                var batch = await _unitOfWork.InventoryBatches.GetByIdAsync(detail.InventoryBatchId);
                if (batch == null || batch.BranchId != dto.BranchId)
                    throw new Exception($"Lô hàng {detail.InventoryBatchId} không hợp lệ hoặc không thuộc chi nhánh {dto.BranchId}.");

                // Lấy sản phẩm
                var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);
                if (product == null)
                    throw new Exception($"Sản phẩm {detail.ProductId} không tồn tại.");

                // Lấy danh sách đơn vị
                var units = await _productService.GetProductUnitsAsync(detail.ProductId);
                if (units == null || !units.Any())
                    throw new Exception($"Sản phẩm {detail.ProductId} không có đơn vị được cấu hình.");

                var unit = units.FirstOrDefault(u => u.Unit == detail.Unit);
                if (unit == null)
                    throw new Exception($"Đơn vị {detail.Unit} không hợp lệ cho sản phẩm {detail.ProductId}.");

                // Tìm đơn vị nhỏ nhất (Viên)
                var smallestUnit = units.OrderByDescending(u => u.ConversionFactor).First();
                var quantityInSmallestUnit = (int)(detail.Quantity * (smallestUnit.ConversionFactor / unit.ConversionFactor));

                // Kiểm tra số lượng lô
                if (quantityInSmallestUnit > batch.Quantity)
                    throw new Exception($"Số lượng vượt quá {batch.Quantity} {smallestUnit.Unit} trong lô {batch.BatchName}.");

                // Trừ số lượng lô chỉ khi không phải Draft
                if (dto.OrderStatus != "Draft")
                {
                    batch.Quantity -= quantityInSmallestUnit;
                    await _unitOfWork.InventoryBatches.UpdateAsync(batch);
                }
            }

            // Lưu đơn hàng
            var order = _mapper.Map<Order>(dto);
            order.Id = Guid.NewGuid();
            order.CustomerName = string.IsNullOrEmpty(dto.CustomerName) ? "Khách lẻ" : dto.CustomerName;
            order.OrderStatus = dto.OrderStatus ?? "Confirmed"; // Mặc định Confirmed nếu null

            foreach (var detail in order.OrderDetails)
            {
                detail.Id = Guid.NewGuid();
                detail.OrderId = order.Id;
                detail.TotalAmount = detail.Quantity * detail.UnitPrice;
            }

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }



        public async Task DeleteAsync(Guid orderId)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetOrderWithDetailsAsync(orderId);
                if (order == null)
                    throw new ArgumentException($"Đơn hàng {orderId} không tồn tại.");

                if (order.OrderStatus != "Draft")
                    throw new ArgumentException("Chỉ có thể xóa đơn hàng ở trạng thái tạm.");

                // Xóa chi tiết đơn hàng
                if (order.OrderDetails.Any())
                {
                    await _unitOfWork.OrderDetails.RemoveRangeAsync(order.OrderDetails);
                }

                // Xóa đơn hàng
                await _unitOfWork.Orders.Delete(order); // Sử dụng DeleteAsync cho thực thể đơn lẻ
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa đơn hàng {OrderId}: {Message}", orderId, ex.Message);
                throw new Exception($"Lỗi khi xóa đơn hàng: {ex.Message}");
            }
        }
        public async Task<PageResultDto<OrderDto>> GetOrderPagedAsync(int page, int pageSize, string? branchId, string? status)
        {
            // Không gọi OrderBy trước, vì sau đó sẽ dùng Where gây lỗi kiểu
            var query = _unitOfWork.Orders.GetAllForPaging()
                .Include(o => o.User)
                .Include(o => o.Branch)
                .AsQueryable();

            if (!string.IsNullOrEmpty(branchId))
            {
                query = query.Where(o => o.BranchId.ToString() == branchId);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.OrderStatus == status);
            }

            // Gọi OrderBy sau khi đã filter xong
            query = query.OrderByDescending(o => o.CreateDate);

            var totalItems = await query.CountAsync();

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageResultDto<OrderDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = _mapper.Map<List<OrderDto>>(orders)
            };
        }


    }
}
