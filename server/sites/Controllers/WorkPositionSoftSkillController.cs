using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class WorkPositionSoftSkillController : WorkPositionJoinController<int, JobChIN_WorkPositionSoftSkill>
    {
        public WorkPositionSoftSkillController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "SoftSkillId";
    }

}