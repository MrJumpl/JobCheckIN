using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mlok.Core.Data;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Models;
using Umbraco.Core.Persistence;
using UmbracoDatabase = Umbraco.Core.Persistence.Database;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class AreaOfInterestController : ModelPagedController<AreaOfInterest, JobChIN_AreaOfInterest, int>, IPickerController<int>
    {
        public AreaOfInterestController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        public override Expression<Func<AreaOfInterest, int>> GetModelId() => areaOfInterest => areaOfInterest.AreaOfInterestId;

        protected override DataProviderSql<JobChIN_AreaOfInterest> GetDataProvider(UmbracoDatabase database) => JobChIN_AreaOfInterest.SelectFromDB(database);

        public IEnumerable<EnumerablePickerValue<int, string>> GetPicker() => GetAll().Select(y => EnumerablePickerValue.From(y.AreaOfInterestId, y.Name.ToString()));

        public override void Delete(IEnumerable<int> ids)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                base.Delete(ids);

                var sql = Sql.Builder.Where("AreaOfInterestId IN (@0)", ids);
                scope.Database.Delete<JobChIN_CompanyAreaOfInterest>(sql);
                scope.Database.Delete<JobChIN_StudentAreaOfInterest>(sql);
                scope.Database.Delete<JobChIN_WorkPositionAreaOfInterest>(sql);
                scope.Complete();
            }
        }
    }
}