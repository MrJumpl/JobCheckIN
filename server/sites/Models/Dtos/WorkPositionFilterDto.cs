using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class WorkPositionFilterDto
    {
        public int PageNo { get; set; }
        public IEnumerable<int> AreaOfInterests { get; set; }
        public IEnumerable<string> Locations { get; set; }
        public IEnumerable<int> ContractTypes { get; set; }
        public string Name { get; set; }
        public IEnumerable<Remote> Remotes { get; set; }
        public IEnumerable<int> Languages { get; set; }
        public OrderBy OrderBy { get; set; }
    }
}