using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
