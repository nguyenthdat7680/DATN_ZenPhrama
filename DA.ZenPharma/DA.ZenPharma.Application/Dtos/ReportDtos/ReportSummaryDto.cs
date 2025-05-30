using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.ReportDtos
{
    public class ReportSummaryDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int NewCustomers { get; set; }
        public int GuestVisits { get; set; }
        public double OrderChangeRate { get; set; }
        public double RevenueChangeRate { get; set; }
        public double CustomerChangeRate { get; set; }
        public double VisitorChangeRate { get; set; }
    }

}
