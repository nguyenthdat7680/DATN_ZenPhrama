using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.CartDto
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public bool IsCheckedOut { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
