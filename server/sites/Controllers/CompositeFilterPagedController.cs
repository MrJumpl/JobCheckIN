using AutoMapper;
using Mlok.Core.Data;
using System.Collections.Generic;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public abstract class CompositeFilterPagedController<TModel, TDBModel, TFilter, TId> : ModelFilterPagedController<TModel, TDBModel, TFilter, TId>
        where TDBModel : DataResult
    {
        protected CompositeFilterPagedController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        public TModel CompositeUpdate(TModel model)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                scope.Database.Update(Mapper.Map<TDBModel>(model));
                scope.Complete();
            }
            return model;
        }

        protected override TModel Save(TModel model)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var columns = GetUpdateColumns(model);
                if (columns.Any())
                    scope.Database.Update(Mapper.Map<TDBModel>(model), GetModelId().Compile().Invoke(model), columns);
                scope.Complete();
            }
            return model;
        }

        protected abstract IEnumerable<string> GetUpdateColumns(TModel model);
    }
}