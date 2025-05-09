using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.CategoryDto
{
    public class CategoryUpdateDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
    }
}
