using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class District : BaseEntity
    {
        public Guid ProvinceId { get; set; }
        public string Name { get; set; }

        public Province Province { get; set; }
        public ICollection<Commune> Communes { get; set; }
    }

}
