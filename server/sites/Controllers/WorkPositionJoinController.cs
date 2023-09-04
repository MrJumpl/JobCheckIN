using Mlok.Core.Data;
using Mlok.Web.Sites.JobChIN.Models;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public abstract class WorkPositionJoinController<TChildId, TDBModel> : JoinController<WorkPosition, int, TChildId, TDBModel>
    {
        protected WorkPositionJoinController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ParentIdLabel => "WorkPositionId";

        protected override int GetParentId(WorkPosition parent) => parent.WorkPositionId;
    }
}