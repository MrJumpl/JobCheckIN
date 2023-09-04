using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class CompanyLanguageSkillPreferredController : CompanyJoinController<int, JobChIN_CompanyLanguageSkillPreferred>
    {
        public CompanyLanguageSkillPreferredController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "LanguageId";
    }
}