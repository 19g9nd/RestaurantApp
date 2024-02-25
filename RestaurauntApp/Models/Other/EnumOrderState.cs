using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace RestaurauntApp.Models.Other
{
    public enum EnumOrderState
    {
        canceled = 0,
        waiting = 1,
        [Display(Name = "in process")]
        in_process = 2,
        [Display(Name = "ready for pick up")]
        ready_for_pickUp = 3,
        completed = 4
    }
     public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            return value.GetType()
                        .GetMember(value.ToString())
                        .FirstOrDefault()?
                        .GetCustomAttribute<DisplayAttribute>()?
                        .GetName() ?? value.ToString();
        }
    }
    
}

