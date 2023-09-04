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
    public class WorkerCountRangeController : ModelPagedController<WorkerCountRange, JobChIN_WorkerCountRange, int>
    {
        public WorkerCountRangeController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        public override Expression<Func<WorkerCountRange, int>> GetModelId() => workerCountRange => workerCountRange.WorkerCountRangeId;

        protected override DataProviderSql<JobChIN_WorkerCountRange> GetDataProvider(UmbracoDatabase database) => JobChIN_WorkerCountRange.SelectFromDB(database);

        public IEnumerable<EnumerablePickerValue<int, string>> GetPicker() => GetAll().Select(y => EnumerablePickerValue.From(y.WorkerCountRangeId, y.GetName()));
    }
}