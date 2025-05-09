using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.InventoryBatchDtos;

namespace DA.ZenPharma.Application.Dtos.ImportInvoiceDetailDtos
{
    public class ImportInvoiceDetailDto
    {
        public Guid Id { get; set; }
        public Guid ImportInvoiceId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string LocationCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProductName { get; set; }
        public List<InventoryBatchDto> InventoryBatches { get; set; } = new();
    }

}
