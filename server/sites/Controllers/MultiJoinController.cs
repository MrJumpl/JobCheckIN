using AutoMapper;
using Mlok.Core.Data;
using Mlok.Umbraco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Umbraco.Core.Persistence;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public abstract class MultiJoinController<TId, TModel, TDBModel>
    {
        protected DbScopeProvider ScopeProvider { get; }

        protected MultiJoinController(DbScopeProvider scopeProvider)
        {
            ScopeProvider = scopeProvider;
        }

        public IEnumerable<TModel> Update(int parentId, IEnumerable<TModel> childs)
        {
            var result = new List<TModel>();
            using (var scope = ScopeProvider.CreateScope())
            {
                var oldValues = GetOldValues(scope.Database, parentId);

                foreach (var oldValue in oldValues)
                {
                    var newValue = childs.FirstOrDefault(x => SameId(x, oldValue));
                    if (newValue == null)
                        Delete(scope.Database, oldValue);
                    else
                    {
                        scope.Database.Update(Mapper.Map(newValue, GetDBModel(parentId)));
                        result.Add(newValue);
                    }
                }
                foreach (var newValue in childs.Where(IsNew))
                {
                    var newModel = Mapper.Map(newValue, GetDBModel(parentId));
                    var idObj = scope.Database.Insert(newModel);
                    TId id = (TId)Convert.ChangeType(idObj, typeof(TId));
                    var memberSelectorExpression = GetModelId().Body as MemberExpression;
                    if (memberSelectorExpression != null)
                    {
                        var property = memberSelectorExpression.Member as PropertyInfo;
                        if (property != null)
                        {
                            property.SetValue(newValue, id, null);
                        }
                    }
                    result.Add(newValue);
                }

                scope.Complete();
            }
            return result;
        }

        protected abstract IEnumerable<TDBModel> GetOldValues(UmbracoDatabase database, int parentId);
        protected abstract bool SameId(TModel model, TDBModel dbModel);
        public abstract Expression<Func<TModel, TId>> GetModelId();
        protected abstract TDBModel GetDBModel(int parentId);

        protected virtual void Delete(UmbracoDatabase database, TDBModel model) => database.Delete(model);

        private bool IsNew(TModel model)
        {
            return GetModelId().Compile()(model)?.Equals(default(TId)) ?? default(TId) == null;
        }
    }
}