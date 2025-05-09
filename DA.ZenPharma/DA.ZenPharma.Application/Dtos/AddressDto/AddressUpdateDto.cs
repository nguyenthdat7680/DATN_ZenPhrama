using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.AddressDto
{
    public class AddressUpdateDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverPhone { get; set; }
        public Guid CommuneId { get; set; }
    }
}
