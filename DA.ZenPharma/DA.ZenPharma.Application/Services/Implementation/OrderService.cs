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

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            var order = _mapper.Map<Order>(dto);

            order.Id = Guid.NewGuid();

            foreach (var detail in order.OrderDetails)
            {
                detail.Id = Guid.NewGuid();
                detail.OrderId = order.Id;
                detail.TotalAmount = detail.Quantity * detail.UnitPrice;
            }

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task UpdateAsync(OrderUpdateDto dto)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(dto.Id);
            if (order == null) return;

            _mapper.Map(dto, order);
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null) return;

            _unitOfWork.Orders.Delete(order);
            await _unitOfWork.SaveChangesAsync();
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
