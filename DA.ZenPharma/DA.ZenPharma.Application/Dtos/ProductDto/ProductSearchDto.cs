using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Enums;
using DA.ZenPharma.WebAppAdmin.Helpers;

namespace DA.ZenPharma.Application.Dtos.ProductDto
{
    public class ProductSearchDto
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        public UnitType Unit { get; set; }
        public string UnitDisplayName { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal? DiscountPrice { get; set; }

    }

}
