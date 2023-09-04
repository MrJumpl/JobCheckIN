using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class StudentHardSkillController : StudentJoinController<int, JobChIN_StudentHardSkill>
    {
        public StudentHardSkillController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "HardSkillId";
    }
}