using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Enums;

namespace DA.ZenPharma.Domain.Entity
{
    public class Product : BaseEntity
    {
        public Guid CategoryId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string BaseUnit { get; set; }
        public string? SKU { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsPrescriptionRequired { get; set; }
        public bool IsPublished { get; set; }
        public string ThumbnailImagePath { get; set; }
        public string Description { get; set; }
        public string? Barcode { get; set; }
        public string? UsageInstructions { get; set; }
        public Category Category { get; set; }
        public List<ProductUnit> ProductUnits { get; set; } = new List<ProductUnit>();
    }

}
