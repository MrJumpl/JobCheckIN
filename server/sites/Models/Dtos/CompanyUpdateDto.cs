using Mlok.Web.Sites.JobChIN.Models.CompanyModels;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class CompanyUpdateDto
    {
        public GeneralInfo GeneralInfo { get; set; }
        public Candidates Candidates { get; set; }
        public PresentationDto Presentation { get; set; }
        public CompanyBranches Branches { get; set; }
    }
}