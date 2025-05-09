using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.PrescriptionDto
{
    public class PrescriptionCreateDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ImagePath { get; set; }
        public string CustomerNote { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
