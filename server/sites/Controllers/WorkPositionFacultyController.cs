using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class WorkPositionFacultyController : WorkPositionJoinController<int, JobChIN_WorkPositionFaculty>
    {
        public WorkPositionFacultyController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "FacultyId";
    }
}