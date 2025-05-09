using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class Province : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<District> Districts { get; set; }
    }

}
