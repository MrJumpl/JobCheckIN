using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mlok.Core.Data;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Models;
using UmbracoDatabase = Umbraco.Core.Persistence.Database;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class ContractTypeController : ModelPagedController<ContractType, JobChIN_ContractType, int>
    {
        public ContractTypeController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        public override Expression<Func<ContractType, int>> GetModelId() => contractType => contractType.ContractTypeId;

        protected override DataProviderSql<JobChIN_ContractType> GetDataProvider(UmbracoDatabase database) => JobChIN_ContractType.SelectFromDB(database);

        public IEnumerable<EnumerablePickerValue<int, string>> GetPicker() => GetAll().Select(y => EnumerablePickerValue.From(y.ContractTypeId, y.Name.ToString()));
    }
}