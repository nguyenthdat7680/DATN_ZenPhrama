using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Entity
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AvatarImagePath { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }

        public Guid? RoleId { get; set; }
        public Role Role { get; set; }
        public Guid? BranchId { get; set; }
        public Branch Branch { get; set; }
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }

}
