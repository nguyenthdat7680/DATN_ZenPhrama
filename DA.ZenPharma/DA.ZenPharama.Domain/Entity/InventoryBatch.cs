using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class InventoryBatch : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string BatchName { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string? LocationCode { get; set; }
        public Guid BranchId { get; set; }
        public Guid ImportInvoiceDetailId { get; set; }
        public int Quantity { get; set; }
        public Branch Branch { get; set; }
        public Product Product { get; set; }
        public ImportInvoiceDetail ImportInvoiceDetail { get; set; }    
    }

}
