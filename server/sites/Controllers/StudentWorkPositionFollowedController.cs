using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class StudentWorkPositionFollowedController : StudentFollowedController<JobChIN_StudentWorkPositionFollowed>
    {
        public StudentWorkPositionFollowedController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string FollowedIdLabel => "WorkPositionId";

        protected override JobChIN_StudentWorkPositionFollowed GetFollowedModel(int studentId, int followedId)
        {
            return new JobChIN_StudentWorkPositionFollowed
            {
                StudentId = studentId,
                WorkPositionId = followedId,
            };
        }
    }
}