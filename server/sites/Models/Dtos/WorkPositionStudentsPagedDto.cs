using Mlok.Core.Models;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class WorkPositionStudentsPagedDto
    {
        public IPaginationInfo PaginationInfo { get; set; }
        public IEnumerable<WorkPositionStudentListViewDto> Students { get; set; }
    }
}