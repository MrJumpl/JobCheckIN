using Mlok.Modules.WebData;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public interface IPickerController<TId>
    {
        /// <summary>
        /// Returns the picker of models.
        /// </summary>
        IEnumerable<EnumerablePickerValue<TId, string>> GetPicker();
    }
}