﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DA.ZenPharma.WebAppAdmin.Helpers
{
    public static class EnumHelper
    {
        public static string GetDisplayName(Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetName() ?? enumValue.ToString();
        }
    }
}
