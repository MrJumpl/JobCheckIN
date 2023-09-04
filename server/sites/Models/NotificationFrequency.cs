using System.ComponentModel.DataAnnotations;

namespace Mlok.Web.Sites.JobChIN.Models
{
    public enum NotificationFrequency
    {
        [Display(Name = "Nikdy")]
        Never = 1,
        [Display(Name = "Ihned")]
        Immediately = 2,
        [Display(Name = "Denně")]
        Daily = 3,
        [Display(Name = "Týdně")]
        Weekly = 4,
    }
}
