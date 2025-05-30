using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Application.Dtos.ProductUnitDtos;

namespace DA.ZenPharma.Application.Dtos.ProductDto
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc")]
        public string ProductCode { get; set; }
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Đơn vị cơ bản là bắt buộc")]
        public string BaseUnit { get; set; } // Đơn vị cơ bản (ví dụ: "Viên")
        public string? SKU { get; set; }
        [Required(ErrorMessage = "Giá gốc là bắt buộc")]
        public decimal RegularPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        public int StockQuantity { get; set; }
        public bool IsPrescriptionRequired { get; set; }
        public bool IsPublished { get; set; }
        public string? ThumbnailImagePath { get; set; }
        public string Description { get; set; }
        public string? Barcode { get; set; }
        public string? UsageInstructions { get; set; }
        [Required(ErrorMessage = "Danh mục là bắt buộc")]
        public Guid CategoryId { get; set; }
        public List<ProductUnitDto> ProductUnits { get; set; } = new List<ProductUnitDto>();
    }
}
