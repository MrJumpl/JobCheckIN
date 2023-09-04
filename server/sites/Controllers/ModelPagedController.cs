using AutoMapper;
using Mlok.Core.Data;
using Mlok.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public abstract class ModelPagedController<TModel, TDBModel, TId> : ModelController<TModel, TDBModel, TId>, IModelPagedController<TModel, TId>
        where TDBModel : DataResult
    {
        protected ModelPagedController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        public virtual IEnumerable<TModel> GetPaged(int pageNo, int itemsPerPage, out IPaginationInfo paginationInfo)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                var page = GetDataProvider(scope.Database)
                    .Paged(pageNo, itemsPerPage)
                    .ExecutePaged();

                paginationInfo = new PaginationInfo()
                {
                    ResultsCount = page.ResultsCount,
                    ResultsPerPage = itemsPerPage,
                    CurrentPage = pageNo,
                    PagesCount = page.PagesCount
                };

                return page.Select(x => Mapper.Map<TModel>(x)).ToList();
            }

        }

    }
}