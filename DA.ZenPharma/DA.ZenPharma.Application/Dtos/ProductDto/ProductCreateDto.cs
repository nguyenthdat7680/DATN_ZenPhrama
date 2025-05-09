using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Enums;

namespace DA.ZenPharma.Application.Dtos.ProductDto
{
    public class ProductCreateDto
    {
        public Guid CategoryId { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public UnitType Unit { get; set; }
        public string? SKU { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsPrescriptionRequired { get; set; }
        public bool IsPublished { get; set; }
        public string ThumbnailImagePath { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Barcode { get; set; }
        public string? UsageInstructions { get; set; }
    }
}
