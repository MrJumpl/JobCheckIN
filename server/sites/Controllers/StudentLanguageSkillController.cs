using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class StudentLanguageSkillController : StudentJoinController<int, JobChIN_StudentLanguageSkill>
    {
        public StudentLanguageSkillController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "LanguageId";
    }
}