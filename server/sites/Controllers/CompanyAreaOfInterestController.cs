using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class CompanyAreaOfInterestController : CompanyJoinController<int, JobChIN_CompanyAreaOfInterest>
    {
        public CompanyAreaOfInterestController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "AreaOfInterestId";
    }
}