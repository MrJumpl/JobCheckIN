using AutoMapper;
using Mlok.Core.Data;
using Mlok.Core.Models;
using System.Collections.Generic;
using System.Linq;
using UmbracoDatabase = Umbraco.Core.Persistence.Database;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public abstract class ModelFilterPagedController<TModel, TDBModel, TFilter, TId> : ModelController<TModel, TDBModel, TId>, IModelFilterPagedController<TModel, TFilter, TId>
        where TDBModel : DataResult
    {
        protected ModelFilterPagedController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        public IEnumerable<TModel> GetPaged(int pageNo, int itemsPerPage, TFilter filter, out IPaginationInfo paginationInfo)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                var page = GetDataProvider(scope.Database, filter)
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
        
        protected abstract DataProviderSql<TDBModel> GetDataProvider(UmbracoDatabase database, TFilter filter);
    }
}