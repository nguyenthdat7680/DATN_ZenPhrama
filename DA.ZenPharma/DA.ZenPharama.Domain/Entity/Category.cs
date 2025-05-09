using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Product> Products { get; set; }
    }

}
