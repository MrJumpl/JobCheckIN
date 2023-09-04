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
    public class SoftSkillController : ModelPagedController<SoftSkill, JobChIN_SoftSkill, int>
    {
        public SoftSkillController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        public override Expression<Func<SoftSkill, int>> GetModelId() => softSkill => softSkill.SoftSkillId;

        protected override DataProviderSql<JobChIN_SoftSkill> GetDataProvider(UmbracoDatabase database) => JobChIN_SoftSkill.SelectFromDB(database);

        public IEnumerable<EnumerablePickerValue<int, string>> GetPicker() => GetAll().Select(y => EnumerablePickerValue.From(y.SoftSkillId, y.Name.ToString()));

        public override void Delete(IEnumerable<int> ids)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                base.Delete(ids);

                var sql = Sql.Builder.Where("SoftSkillId IN (@0)", ids);
                scope.Database.Delete<JobChIN_StudentSoftSkill>(sql);
                scope.Database.Delete<JobChIN_WorkPositionSoftSkill>(sql);
                scope.Complete();
            }
        }
    }
}