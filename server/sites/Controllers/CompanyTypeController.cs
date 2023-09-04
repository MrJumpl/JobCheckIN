using System;
using System.Linq.Expressions;
using Mlok.Core.Data;
using Mlok.Web.Sites.JobChIN.Models;
using UmbracoDatabase = Umbraco.Core.Persistence.Database;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class CompanyTypeController : ModelPagedController<CompanyType, JobChIN_CompanyType, int>
    {
        public CompanyTypeController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        public override Expression<Func<CompanyType, int>> GetModelId() => companyType => companyType.CompanyTypeId;

        protected override DataProviderSql<JobChIN_CompanyType> GetDataProvider(UmbracoDatabase database) => JobChIN_CompanyType.SelectFromDB(database);
    }
}