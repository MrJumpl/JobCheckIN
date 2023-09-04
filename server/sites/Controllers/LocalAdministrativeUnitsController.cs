using Mlok.Core.Data;
using Mlok.Modules.WebData;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class LocalAdministrativeUnitsController : IPickerController<string>
    {
        private readonly DbScopeProvider scopeProvider;

        public LocalAdministrativeUnitsController(DbScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        public IEnumerable<EnumerablePickerValue<string, string>> GetPicker()
        {
            using (var scope = scopeProvider.CreateReadOnlyScope())
            {
                return Adresy_CR_Okresy.SelectFromDB(scope.Database)
                    .Execute()
                    .Select(y => EnumerablePickerValue.From(y.Kod_LAU1, y.Nazev));
            }
        }

        public string GetName(string locationId)
        {
            if (locationId.IsNullOrWhiteSpace())
                return null;
            using (var scope = scopeProvider.CreateReadOnlyScope())
            {
                return Adresy_CR_Okresy.SelectFromDB(scope.Database)
                    .Where(x => x.Kod_LAU1 == locationId)
                    .SingleOrDefault()
                    ?.Nazev;
            }
        }
    }
}