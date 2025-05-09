using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Application.Dtos.LoginDto
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
