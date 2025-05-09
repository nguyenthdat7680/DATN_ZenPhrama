using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.ImportInvoiceDtos
{
    public class ImportInvoiceUpdateDto
    {
        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; }
        public Guid BranchLocationId { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid HandledByStaffId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
