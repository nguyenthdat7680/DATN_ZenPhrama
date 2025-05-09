using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class Order : BaseEntity
    {
        public string? OrderCode { get; set; }
        public Guid? UserId { get; set; }
        public Guid? HandledById { get; set; }
        public Guid? AddressId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid BranchId { get; set; }
        public string? CustomerName { get; set; }  
        public string? CustomerPhone { get; set; }
        public User? User { get; set; }
        public User? HandledBy { get; set; }
        public Address? Address { get; set; }
        public Branch Branch { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }


}
