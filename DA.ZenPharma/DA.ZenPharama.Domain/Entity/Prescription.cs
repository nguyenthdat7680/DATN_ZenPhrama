using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class Prescription : BaseEntity
    {
        public Guid UserId { get; set; } 

        public string ImageUrl { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }

        public User User { get; set; } 
    }


}
