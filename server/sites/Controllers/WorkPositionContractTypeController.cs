using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class WorkPositionContractTypeController : WorkPositionJoinController<int, JobChIN_WorkPositionContractType>
    {
        public WorkPositionContractTypeController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "ContractTypeId";
    }
}