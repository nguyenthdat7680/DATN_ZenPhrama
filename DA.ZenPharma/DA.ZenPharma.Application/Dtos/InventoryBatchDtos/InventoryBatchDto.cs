using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.InventoryBatchDtos
{
    public class InventoryBatchDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid ImportInvoiceDetailId { get; set; }

        public string? BatchName { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string? LocationCode { get; set; }
        public int? Quantity { get; set; }

        public string? ProductName { get; set; } 
    }
}
