using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class WorkPositionLanguageSkillController : WorkPositionJoinController<int, JobChIN_WorkPositionLanguageSkill>
    {
        public WorkPositionLanguageSkillController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "LanguageId";
    }

}