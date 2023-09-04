using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class StudentPreferredContractTypeController : StudentJoinController<int, JobChIN_StudentPreferredContractType>
    {
        public StudentPreferredContractTypeController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ChildIdLabel => "ContractTypeId";
    }
}