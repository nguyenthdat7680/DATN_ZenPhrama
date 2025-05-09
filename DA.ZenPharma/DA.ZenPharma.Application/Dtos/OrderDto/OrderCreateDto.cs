using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.OrderDetailDtos;

namespace DA.ZenPharma.Application.Dtos.OrderDto
{
    public class OrderCreateDto
    {
        public string? OrderCode { get; set; }
        public Guid? HandledById { get; set; }
        public decimal TotalAmount { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public Guid BranchId { get; set; }
        public List<OrderDetailCreateDto> Details { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
