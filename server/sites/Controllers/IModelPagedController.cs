using Mlok.Core.Models;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public interface IModelPagedController<TModel, TId> : IModelController<TModel, TId>
    {
        /// <summary>
        /// Returns the page of the models. Only given page from database.
        /// </summary>
        /// <param name="pageNo">Number of the page.</param>
        /// <param name="itemsPerPage">Items per page.</param>
        /// <param name="paginationInfo">Out pagination info about the given page.</param>
        IEnumerable<TModel> GetPaged(int pageNo, int itemsPerPage, out IPaginationInfo paginationInfo);
    }
}