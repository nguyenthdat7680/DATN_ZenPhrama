using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class Address : BaseEntity
    {
        public Guid UserId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public Guid CommuneId { get; set; }
        public bool IsPrimary { get; set; }

        public User User { get; set; }
        public Commune Commune { get; set; }
    }

}
