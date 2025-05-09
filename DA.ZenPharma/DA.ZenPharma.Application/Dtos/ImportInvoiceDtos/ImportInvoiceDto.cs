using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.ImportInvoiceDetailDtos;

namespace DA.ZenPharma.Application.Dtos.ImportInvoiceDtos
{
    public class ImportInvoiceDto
    {
        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; }
        public Guid BranchLocationId { get; set; }
        public string BranchName { get; set; }
        public Guid SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid HandledByStaffId { get; set; }
        public string HandledByStaffName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ImportInvoiceDetailDto> Details { get; set; } = new();
    }

}
