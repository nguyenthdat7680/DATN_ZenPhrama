using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.ReportDtos
{
    public class TopSellingProductDto
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal Price { get; set; }
    }
}
