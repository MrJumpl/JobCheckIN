using Mlok.Core.Data;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using System.Collections.Generic;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class FacultyController : IPickerController<string>
    {
        public IEnumerable<EnumerablePickerValue<string, string>> GetPicker() => MUPartsCache.Current.GetAll()
            .Where(x => x.IsFaculty)
            .Select(y => EnumerablePickerValue.From(y.DepartmentId, this.Localize(y.NameCs, y.NameEn)));
    }
}