using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.OrderDetailDtos;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDetailDto>> GetAllAsync()
        {
            var orderDetails = await _unitOfWork.OrderDetails.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDetailDto>>(orderDetails);
        }

        public async Task<OrderDetailDto?> GetByIdAsync(Guid id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id);
            return orderDetail == null ? null : _mapper.Map<OrderDetailDto>(orderDetail);
        }

        public async Task AddAsync(OrderDetailCreateDto dto)
        {
            var orderDetail = _mapper.Map<OrderDetail>(dto);
            await _unitOfWork.OrderDetails.AddAsync(orderDetail);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderDetailUpdateDto dto)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(dto.Id);
            if (orderDetail == null) return;

            _mapper.Map(dto, orderDetail);
            await _unitOfWork.OrderDetails.UpdateAsync(orderDetail);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id);
            if (orderDetail == null) return;

            _unitOfWork.OrderDetails.Delete(orderDetail);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
