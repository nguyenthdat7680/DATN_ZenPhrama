using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.ZenPharma.Domain.Enums
{
    public enum UnitType
    {
        [Display(Name = "Viên")]
        Vien = 1,

        [Display(Name = "Hộp")]
        Hop = 2,

        [Display(Name = "Chai")]
        Chai = 3,

        [Display(Name = "Tuýp")]
        Tuyp = 4,

        [Display(Name = "Gói")]
        Goi = 5
    }
}
