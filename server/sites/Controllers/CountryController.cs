using Mlok.Core.Data;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class CountryController : IPickerController<int>
    {
        private readonly DbScopeProvider scopeProvider;

        public CountryController(DbScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        public IEnumerable<Staty> GetCountries()
        {
            using (var scope = scopeProvider.CreateReadOnlyScope())
            {
                return Staty.SelectFromDB()
                    .Where(x => x.Kod2 != null && x.Kod3 != null)
                    .Execute()
                    .OrderBy(CountryOrder)
                    .ToList();
            }
        }

        public IEnumerable<EnumerablePickerValue<int, string>> GetPicker() => GetCountries().Select(y => EnumerablePickerValue.From(y.Kod, this.Localize(y.Nazev_cs, y.Nazev_en)));

        private int CountryOrder(Staty country)
        {
            if (country.Kod == SiteConstants.CzechCountryId)
                return 0;
            if (Convert.ToBoolean(country.Casto_Pouzivane))
                return 1;
            return 2;
        }
    }
}