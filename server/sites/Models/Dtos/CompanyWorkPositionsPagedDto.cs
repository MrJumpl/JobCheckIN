using Mlok.Core.Models;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class CompanyWorkPositionsPagedDto
    {
        public IPaginationInfo PaginationInfo { get; set; }
        public IEnumerable<CompanyWorkPositionListViewDto> WorkPositions { get; set; }
    }
}