using System.ComponentModel.DataAnnotations;

namespace Mlok.Web.Sites.JobChIN.Models
{
    public enum Remote
    {
        [Display(Name = "Žádná práce z domu")]
        No = 1,
        [Display(Name = "Částečná práce z domu")]
        Partial= 2,
        [Display(Name = "Práce z domu")]
        Full = 3,
    }
}
