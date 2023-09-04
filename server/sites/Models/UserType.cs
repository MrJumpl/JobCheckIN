using System;
using System.ComponentModel.DataAnnotations;

namespace Mlok.Web.Sites.JobChIN.Models
{
    [Flags]
    public enum UserType
    {
        [Display(Name = "Studenti")]
        Student = 1 << 0,
        [Display(Name = "Zaměstnavatelé")]
        Company = 1 << 1,
    }
}
