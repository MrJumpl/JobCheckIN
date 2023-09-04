using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class WorkPositionHardSkillController : WorkPositionJoinController<int, JobChIN_WorkPositionHardSkill>
    {
        public WorkPositionHardSkillController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "HardSkillId";
    }
}