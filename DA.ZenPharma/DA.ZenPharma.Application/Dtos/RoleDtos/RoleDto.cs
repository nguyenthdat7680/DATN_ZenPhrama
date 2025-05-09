using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA.ZenPharma.Domain.Entity;

namespace DA.ZenPharma.Application.Dtos.RoleDtos
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

    }
}
