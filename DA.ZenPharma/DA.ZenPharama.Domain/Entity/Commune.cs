using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class Commune : BaseEntity
    {
        public Guid DistrictId { get; set; }
        public string Name { get; set; }

        public District District { get; set; }
    }

}
