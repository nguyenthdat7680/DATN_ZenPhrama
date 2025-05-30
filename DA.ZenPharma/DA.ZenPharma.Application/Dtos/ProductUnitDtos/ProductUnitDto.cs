using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.ProductUnitDtos
{
    public class ProductUnitDto
    {
        [Required(ErrorMessage = "Đơn vị là bắt buộc")]
        public string Unit { get; set; } // Đơn vị (ví dụ: "Hộp")
        [Required(ErrorMessage = "Hệ số quy đổi là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Hệ số quy đổi phải lớn hơn 0")]
        public decimal ConversionFactor { get; set; }
    }
}
