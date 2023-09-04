using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class WorkPositionAreaOfInterestController : WorkPositionJoinController<int, JobChIN_WorkPositionAreaOfInterest>
    {
        public WorkPositionAreaOfInterestController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "AreaOfInterestId";
    }
}