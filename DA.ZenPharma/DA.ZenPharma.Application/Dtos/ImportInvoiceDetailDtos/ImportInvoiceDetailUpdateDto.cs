using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.ImportInvoiceDetailDtos
{
    public class ImportInvoiceDetailUpdateDto
    {
        public Guid Id { get; set; }
        public Guid ImportInvoiceId { get; set; }
        public Guid ProductId { get; set; }
        public Guid Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string LocationCode { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
