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
    public class LanguageController : ModelPagedController<Language, JobChIN_Language, int>
    {
        public LanguageController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        public IEnumerable<Tuple<T, string>> GetDisplayLanguages<T>(IEnumerable<T> models)
            where T : LanguageModel
        {
            var languages = GetByIds(models?.Select(x => x.LanguageId.Value));
            if (languages.Any())
                return languages.Select(x => GetLang(models, x));
            return Enumerable.Empty<Tuple<T, string>>();
        }

        public override Expression<Func<Language, int>> GetModelId() => softSkill => softSkill.LanguageId;

        protected override DataProviderSql<JobChIN_Language> GetDataProvider(UmbracoDatabase database) => JobChIN_Language.SelectFromDB(database);

        public IEnumerable<EnumerablePickerValue<int?, string>> GetPicker() => GetAll().Select(y => EnumerablePickerValue.From<int?, string>(y.LanguageId, y.Name.ToString()));

        private Tuple<T, string> GetLang<T>(IEnumerable<T> models, Language lang)
            where T : LanguageModel
        {
            var model = models.First(x => x.LanguageId == lang.LanguageId);
            return new Tuple<T, string>(model, $"{lang.Name} – {model.Skill}");
        }
    }
}