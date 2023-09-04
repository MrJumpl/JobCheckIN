using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class CompanyFacultyController : CompanyJoinController<int, JobChIN_CompanyFaculty>
    {
        public CompanyFacultyController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "FacultyId";
    }
}