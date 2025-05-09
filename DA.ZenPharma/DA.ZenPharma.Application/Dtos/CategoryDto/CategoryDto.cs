using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.CategoryDto
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
