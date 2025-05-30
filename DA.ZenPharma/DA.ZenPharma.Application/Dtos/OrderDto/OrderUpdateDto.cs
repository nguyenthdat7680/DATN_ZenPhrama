using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.OrderDetailDtos;

namespace DA.ZenPharma.Application.Dtos.OrderDto
{
    public class OrderUpdateDto
    {
        public Guid Id { get; set; }
        public Guid BranchId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public Guid HandledById { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderDetailUpdateDto> Details { get; set; }
    }
}
