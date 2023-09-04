using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class StudentSoftSkillController : StudentJoinController<int, JobChIN_StudentSoftSkill>
    {
        public StudentSoftSkillController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "SoftSkillId";
    }

}