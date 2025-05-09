using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }
        public string Description { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
