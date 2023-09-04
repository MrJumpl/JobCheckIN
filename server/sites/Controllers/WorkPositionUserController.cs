using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class WorkPositionUserController : WorkPositionJoinController<int, JobChIN_WorkPositionUser>
    {
        public WorkPositionUserController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "CompanyUserId";
    }
}