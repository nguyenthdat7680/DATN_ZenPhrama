using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.ReportDtos
{
    public class RevenueByDateDto
    {
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
