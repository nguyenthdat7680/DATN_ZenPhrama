using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.OrderDetailDtos
{
    public class OrderDetailUpdateDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid InventoryBatchId { get; set; }
        public int Quantity { get; set; }
        public string? Unit { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
