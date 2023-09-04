using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class CompanyAngularConfigDto
    {
        public int CompanyId { get; set; }
        public bool Confirmed { get; set; }
        public int MaxDuration { get; set; }
        public CompanyTypeConfigDto CompanyTypeConfig { get; set; }
        public CompanyUpdateDto Model { get; set; }
        public IEnumerable<CompanyUserViewDto> Users { get; set; }
        public CompanyUserUpdateDto User { get; set; }
        public Role Role { get; set; }
        public int MemberId { get; set; }
    }
}