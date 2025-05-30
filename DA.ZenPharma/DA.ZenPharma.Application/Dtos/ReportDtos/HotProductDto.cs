using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.ReportDtos
{
    public class HotProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int TotalSold { get; set; }
    }
}
