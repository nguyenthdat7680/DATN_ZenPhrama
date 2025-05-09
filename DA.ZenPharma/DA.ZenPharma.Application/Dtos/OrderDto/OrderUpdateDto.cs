using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.OrderDto
{
    public class OrderUpdateDto
    {
        public Guid Id { get; set; }
        public string? OrderCode { get; set; }
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public int? UserId { get; set; }
        public int OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
