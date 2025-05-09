using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.CartDto;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartDto>> GetAllAsync()
        {
            var carts = await _unitOfWork.Carts.GetAllAsync();
            return _mapper.Map<IEnumerable<CartDto>>(carts);
        }

        public async Task<CartDto?> GetByIdAsync(Guid id)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(id);
            return cart == null ? null : _mapper.Map<CartDto>(cart);
        }

        public async Task AddAsync(CartCreateDto dto)
        {
            var cart = _mapper.Map<Cart>(dto);
            await _unitOfWork.Carts.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(CartUpdateDto dto)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(dto.Id);
            if (cart == null) return;

            _mapper.Map(dto, cart);
            _unitOfWork.Carts.UpdateAsync(cart);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(id);
            if (cart == null) return;

            _unitOfWork.Carts.Delete(cart);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
