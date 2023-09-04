using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class CompanyTypeConfigDto
    {
        public string AfterLogin { get; set; }
        public IEnumerable<CompanyCompanyTypeDto> CurrentCompanyTypes { get; set; }
        public IEnumerable<CompanyTypeDto> CompanyTypes { get; set; }
    }
}