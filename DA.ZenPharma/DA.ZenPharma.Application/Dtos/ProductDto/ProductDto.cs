using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.ProductDto
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string SKU { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsPrescriptionRequired { get; set; }
        public bool IsPublished { get; set; }
        public string ThumbnailImagePath { get; set; }
        public string Description { get; set; }
        public string SeoAlias { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }

}
