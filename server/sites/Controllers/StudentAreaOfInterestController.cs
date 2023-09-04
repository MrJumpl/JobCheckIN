using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class StudentAreaOfInterestController : StudentJoinController<int, JobChIN_StudentAreaOfInterest>
    {
        public StudentAreaOfInterestController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "AreaOfInterestId";
    }
}