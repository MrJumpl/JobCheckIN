using System.ComponentModel.DataAnnotations;

namespace Mlok.Web.Sites.JobChIN.Models
{
    public enum OrderBy
    {
        [Display(Name = "Datum")]
        Date = 1,
        [Display(Name = "Shoda")]
        Match = 2,
    }
}
