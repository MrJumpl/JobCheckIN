using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mlok.Core.Data;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Models;
using Mlok.Web.Sites.JobChIN.Models.Filters;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using UmbracoDatabase = Umbraco.Core.Persistence.Database;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class HardSkillController : ModelFilterPagedController<HardSkill, JobChIN_HardSkill, HardSkillFilter, int>
    {
        public HardSkillController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        public override Expression<Func<HardSkill, int>> GetModelId() => hardSkill => hardSkill.HardSkillId;

        protected override DataProviderSql<JobChIN_HardSkill> GetDataProvider(UmbracoDatabase database, HardSkillFilter filter)
        {
            var provider = JobChIN_HardSkill.SelectFromDB(database);

            if (!filter.SkillName.IsNullOrWhiteSpace())
                provider = provider.WhereGroup(groupProvider => groupProvider
                    .OrWhere(x => x.NameCs.Contains(filter.SkillName))
                    .OrWhere(x => x.NameEn.Contains(filter.SkillName)));

            if (filter.AreaOfInterestId.HasValue)
                provider = provider.Where(x => x.AreaOfInterestId == filter.AreaOfInterestId);

            if (!filter.MuPartId.IsNullOrWhiteSpace())
            {
                var sufixStartIndex = filter.MuPartId.IndexOf('0');
                var prefix = sufixStartIndex < 0 ? filter.MuPartId : filter.MuPartId.Remove(sufixStartIndex);
                provider = provider.Where(x => x.MuPartId.StartsWith(prefix));
            }

            return provider;
        }

        protected override DataProviderSql<JobChIN_HardSkill> GetDataProvider(UmbracoDatabase database) => JobChIN_HardSkill.SelectFromDB(database);

        public IEnumerable<EnumerablePickerValue<int, string>> GetPicker() => GetAll().Select(y => EnumerablePickerValue.From(y.HardSkillId, y.Name.ToString()));

        public override void Delete(IEnumerable<int> ids)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                base.Delete(ids);

                var sql = Sql.Builder.Where("HardSkillId IN (@0)", ids);
                scope.Database.Delete<JobChIN_StudentHardSkill>(sql);
                scope.Database.Delete<JobChIN_WorkPositionHardSkill>(sql);
                scope.Complete();
            }
        }
    }
}