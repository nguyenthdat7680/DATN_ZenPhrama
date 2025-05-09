using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;
using DA.ZenPharma.Application.Dtos.OrderDetailDtos;

namespace DA.ZenPharma.Application.Dtos.OrderDto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string? OrderCode { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? AddressId { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid? UserId { get; set; }
        public string? OrderStatus { get; set; }
        public Guid BranchId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? BranchName { get; set; }
        public string? HandledBy { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new();
    }
    
}
