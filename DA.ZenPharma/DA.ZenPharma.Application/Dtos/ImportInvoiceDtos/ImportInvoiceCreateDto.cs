using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.ImportInvoiceDetailDtos;

namespace DA.ZenPharma.Application.Dtos.ImportInvoiceDtos
{
    public class ImportInvoiceCreateDto
    {
        public string? InvoiceNumber { get; set; } 
        public Guid BranchLocationId { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime? InvoiceDate { get; set; } 
        public decimal? TotalAmount { get; set; }  
        public Guid HandledByStaffId { get; set; }

        public List<ImportInvoiceDetailCreateDto> Details { get; set; } = new();
    }

}
