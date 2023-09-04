using System;
using System.ComponentModel.DataAnnotations;

namespace Mlok.Web.Sites.JobChIN.Models.CompanyModels
{
    [Flags]
    public enum Role
    {
        [Display(Name = "Správce firmy")]
        CompanyAdmin = 1 << 0,
        [Display(Name = "Správce pracovní pozice")]
        WorkPositionAdmin = 1 << 1,
    }
}