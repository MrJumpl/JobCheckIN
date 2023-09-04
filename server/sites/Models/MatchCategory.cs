using System.ComponentModel.DataAnnotations;

namespace Mlok.Web.Sites.JobChIN.Models
{
    public enum MatchCategory
    {
        [Display(Name = "Doporučujeme")]
        Recommend = 1,
        [Display(Name = "Mohlo by Vás zajímat")]
        Match = 2,
        [Display(Name = "Splňujete")]
        NoMatch = 3,
        [Display(Name = "Nesplňujete")]
        NotSuitable = 4,
    }
}
