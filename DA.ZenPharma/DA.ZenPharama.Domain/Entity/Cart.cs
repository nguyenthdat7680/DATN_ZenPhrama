using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class Cart : BaseEntity
    {
        public Guid UserId { get; set; } // đại diện cho Customer

        public User User { get; set; }
        public ICollection<CartItem> Items { get; set; }
    }


}
