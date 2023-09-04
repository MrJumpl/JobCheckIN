using AutoMapper;
using Mlok.Core.Data;
using Mlok.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using UmbracoDatabase = Umbraco.Core.Persistence.Database;

namespace Mlok.Web.Sites.JobChIN.Controllers
{

    public abstract class ModelController<TModel, TDBModel, TId> : IModelController<TModel, TId>
        where TDBModel : DataResult
    {
        protected DbScopeProvider ScopeProvider { get; }

        protected ModelController(DbScopeProvider scopeProvider)
        {
            ScopeProvider = scopeProvider;
        }

        public virtual void Delete(IEnumerable<TId> ids)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                foreach (var id in ids)
                    scope.Database.Delete<TDBModel>(id);
                scope.Complete();
            }
        }

        public virtual TModel GetById(TId id)
        {
            TDBModel dbModel = null;
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                dbModel = scope.Database.SingleOrDefault<TDBModel>(id);
            }
            if (dbModel != null)
                return Mapper.Map<TModel>(dbModel);
            return default(TModel);
        }

        public virtual IEnumerable<TModel> GetByIds(IEnumerable<TId> ids)
        {
            if (!(ids?.Any() ?? false))
                return Enumerable.Empty<TModel>();
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                return GetDataProvider(scope.Database)
                    .Where(GetPrimaryKeyExpression(), SqlCompareType.In, ids)
                    .Execute().Select(Mapper.Map<TModel>).ToList();
            }
        }

        public abstract Expression<Func<TModel, TId>> GetModelId();

        public TModel Update(TModel model)
        {
            if (GetModelId().Compile()(model)?.Equals(default(TId)) ?? default(TId) == null)
                return Insert(model);
            return Save(model);
        }

        public IEnumerable<TModel> GetAll()
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                return GetDataProvider(scope.Database).Execute()
                    .Select(x => Mapper.Map<TModel>(x))
                    .ToList();
            }
        }

        public IEnumerable<T> GetAll<T>() => GetAll().Select(x => Mapper.Map<T>(x));

        protected virtual TModel Insert(TModel model)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                object idObj = scope.Database.Insert(Mapper.Map<TDBModel>(model));
                TId id = (TId)Convert.ChangeType(idObj, typeof(TId));
                var memberSelectorExpression = GetModelId().Body as MemberExpression;
                if (memberSelectorExpression != null)
                {
                    var property = memberSelectorExpression.Member as PropertyInfo;
                    if (property != null)
                    {
                        property.SetValue(model, id, null);
                    }
                }
                scope.Complete();
            }

            return model;
        }

        protected virtual TModel Save(TModel model)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                scope.Database.Save(Mapper.Map<TDBModel>(model));
                scope.Complete();
            }
            return model;
        }

        protected abstract DataProviderSql<TDBModel> GetDataProvider(UmbracoDatabase database);


        Expression<Func<TDBModel, TId>> GetPrimaryKeyExpression()
        {
            var attr = typeof(TDBModel).FirstAttribute<PrimaryKeyAttribute>();
            var parameter = Expression.Parameter(typeof(TDBModel), attr.Value);
            var propertyAccess = Expression.Property(parameter, typeof(TDBModel).GetProperty(attr.Value));
            var box = Expression.Convert(propertyAccess, typeof(TId));

            return Expression.Lambda<Func<TDBModel, TId>>(box, parameter);
        }
    }
}