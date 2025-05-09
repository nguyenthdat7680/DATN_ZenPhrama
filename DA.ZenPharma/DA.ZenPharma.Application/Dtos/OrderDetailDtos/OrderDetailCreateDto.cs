using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.OrderDetailDtos
{
    public class OrderDetailCreateDto
    {
        public Guid ProductId { get; set; }
        public Guid InventoryBatchId { get; set; }  
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
