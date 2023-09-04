using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class StudentCompanyFollowedController : StudentFollowedController<JobChIN_StudentCompanyFollowed>
    {
        public StudentCompanyFollowedController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string FollowedIdLabel => "CompanyId";

        protected override JobChIN_StudentCompanyFollowed GetFollowedModel(int studentId, int followedId)
        {
            return new JobChIN_StudentCompanyFollowed
            {
                StudentId = studentId,
                CompanyId = followedId,
            };
        }
    }
}