using Mlok.Core.Data;
using Mlok.Web.Sites.JobChIN.Models;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public abstract class CompanyJoinController<TChildId, TDBModel> : JoinController<Company, int, TChildId, TDBModel>
    {
        protected CompanyJoinController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ParentIdLabel => "CompanyId";

        protected override int GetParentId(Company parent) => parent.CompanyId;
    }
}