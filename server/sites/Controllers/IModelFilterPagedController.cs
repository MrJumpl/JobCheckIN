using Mlok.Core.Models;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public interface IModelFilterPagedController<TModel, TFilter, TId> : IModelController<TModel, TId>
    {
        /// <summary>
        /// Returns the page of the models. Data are filtered on database layer and gets only given page from database.
        /// </summary>
        /// <param name="pageNo">Number of the page.</param>
        /// <param name="itemsPerPage">Items per page.</param>
        /// <param name="filter">Filter model that efects the sql query.</param>
        /// <param name="paginationInfo">Out pagination info about the given page.</param>
        IEnumerable<TModel> GetPaged(int pageNo, int itemsPerPage, TFilter filter, out IPaginationInfo paginationInfo);
    }
}