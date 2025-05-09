using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.InventoryBatchDtos;

namespace DA.ZenPharma.Application.Dtos.ImportInvoiceDetailDtos
{
    public class ImportInvoiceDetailCreateDto
    {
        public Guid ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalAmount { get; set; } 

        public List<InventoryBatchCreateDto> Batches { get; set; } = new();
    }

}
