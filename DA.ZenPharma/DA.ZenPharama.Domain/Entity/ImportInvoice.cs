using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class ImportInvoice : BaseEntity
    {
        public string? InvoiceNumber { get; set; }
        public Guid BranchLocationId { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid? HandledByStaffId { get; set; }
        public Branch Branch { get; set; }
        public Supplier Supplier { get; set; }
        public User? Staff { get; set; }

        public ICollection<ImportInvoiceDetail> Details { get; set; } = new List<ImportInvoiceDetail>();
    }

}
