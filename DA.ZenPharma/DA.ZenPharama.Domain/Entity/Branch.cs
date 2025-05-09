using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class Branch : BaseEntity
    {
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<ImportInvoice> ImportInvoices { get; set; }
        public ICollection<InventoryBatch> InventoryBatches { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

}
