using System;
using System.Linq.Expressions;
using Mlok.Core.Data;
using Mlok.Web.Sites.JobChIN.Models;
using UmbracoDatabase = Umbraco.Core.Persistence.Database;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class HardSkillSuggestController : ModelPagedController<HardSkillSuggest, JobChIN_HardSkillSuggest, int>
    {
        public HardSkillSuggestController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        public override Expression<Func<HardSkillSuggest, int>> GetModelId() => hardSkillSuggest => hardSkillSuggest.HardSkillSuggestId;

        protected override DataProviderSql<JobChIN_HardSkillSuggest> GetDataProvider(UmbracoDatabase database) => JobChIN_HardSkillSuggest.SelectFromDB(database);
    }
}