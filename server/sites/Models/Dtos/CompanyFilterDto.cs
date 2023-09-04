using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class CompanyFilterDto
    {
        public string CompanyName { get; set; }
        public IEnumerable<int> AreaOfInterests { get; set; }

    }
}