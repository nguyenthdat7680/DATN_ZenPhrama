using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Enums;

namespace DA.ZenPharma.Domain.Entity
{
    public class ProductUnit
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Unit { get; set; } // Đơn vị (ví dụ: "Hộp")
        public decimal ConversionFactor { get; set; } // Hệ số quy đổi (ví dụ: 1 Hộp = 10 Viên)
        public Product Product { get; set; }
    }
}
