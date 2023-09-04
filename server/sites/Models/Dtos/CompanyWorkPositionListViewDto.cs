using System;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class CompanyWorkPositionListViewDto
    {
        public int WorkPositionId { get; set; }
        public string Name { get; set; }
        public int ShownInterestCount { get; set; }
        public int ViewsCount { get; set; }
        public DateTime Publication { get; set; }
        public DateTime Expiration { get; set; }
    }
}