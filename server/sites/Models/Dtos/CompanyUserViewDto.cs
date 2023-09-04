using Mlok.Web.Sites.JobChIN.Models.CompanyModels;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class CompanyUserViewDto
    {
        public int MemberId { get; set; }
        public string FullName { get; set; }
        public Role Role { get; set; }
    }
}