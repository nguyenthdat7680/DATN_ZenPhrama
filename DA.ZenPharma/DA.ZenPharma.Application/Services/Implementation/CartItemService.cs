using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DA.ZenPharma.Application.Dtos.CartItemDto;
using DA.ZenPharma.Application.Services.Interfaces;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Infrastructure.UnitOfWork;

namespace DA.ZenPharma.Application.Services.Implementation
{
    public class CartItemService : ICartItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartItemDto>> GetAllAsync()
        {
            var cartItems = await _unitOfWork.CartItems.GetAllAsync();
            return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
        }

        public async Task<CartItemDto?> GetByIdAsync(Guid id)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(id);
            return cartItem == null ? null : _mapper.Map<CartItemDto>(cartItem);
        }

        public async Task AddAsync(CartItemCreateDto dto)
        {
            var cartItem = _mapper.Map<CartItem>(dto);
            await _unitOfWork.CartItems.AddAsync(cartItem);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(CartItemUpdateDto dto)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(dto.Id);
            if (cartItem == null) return;

            _mapper.Map(dto, cartItem);
            _unitOfWork.CartItems.UpdateAsync(cartItem);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var cartItem = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (cartItem == null) return;

            _unitOfWork.CartItems.Delete(cartItem);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
