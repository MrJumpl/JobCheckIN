using System.ComponentModel.DataAnnotations;

namespace Mlok.Web.Sites.JobChIN.Models
{
    public enum Sector
    {
        [Display(Name = "Soukromý")]
        PrivateSector = 1,
        [Display(Name = "Neziskový")]
        NonProfitSector = 2,
        [Display(Name = "Veřejný")]
        PublicSector = 3,
    }
}
