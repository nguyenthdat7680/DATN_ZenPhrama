using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class OrderDetail : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string Unit { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public Guid? InventoryBatchId { get; set; }
    }

}
