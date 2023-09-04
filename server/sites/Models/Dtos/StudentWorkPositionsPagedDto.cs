using Mlok.Core.Models;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class StudentWorkPositionsPagedDto
    {
        public IPaginationInfo PaginationInfo { get; set; }
        public IEnumerable<StudentWorkPositionListViewDto> WorkPositions { get; set; }
    }
}