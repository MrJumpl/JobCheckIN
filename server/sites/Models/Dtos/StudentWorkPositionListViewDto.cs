using System;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class StudentWorkPositionListViewDto
    {
        public int WorkPositionId { get; set; }
        public string Name { get; set; }
        public string LocationId { get; set; }
        public IEnumerable<int> ContractTypes { get; set; }
        public Remote Remote { get; set; }
        public DateTime Publication { get; set; }
        public DateTime Expiration { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public MatchCategory Match { get; set; }
        public bool Favorite { get; set; }
    }
}