using AutoMapper;
using Mlok.Core.Data;
using System.Collections.Generic;
using Umbraco.Core.Persistence;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public abstract class JoinController<TParent, TParentId, TChildId, TJoinDBModel> : IJoinController<TParent, TParentId, TChildId>
    {
        protected DbScopeProvider ScopeProvider { get; }

        protected abstract string ParentIdLabel { get; }
        protected abstract string ChildIdLabel { get; }

        protected JoinController(DbScopeProvider scopeProvider)
        {
            ScopeProvider = scopeProvider;
        }

        public virtual void Join(TParent parent)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                DeleteByParentId(GetParentId(parent));
                foreach (var child in Mapper.Map<IEnumerable<TJoinDBModel>>(parent))
                    scope.Database.Insert(child);
                scope.Complete();
            }
        }

        public virtual void DeleteByParentId(TParentId parentId)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var sqlBase = Sql.Builder
                       .Where($"{ParentIdLabel} = @0", parentId);
                scope.Database.Delete<TJoinDBModel>(sqlBase);
                scope.Complete();
            }

        }

        public virtual void DeleteByChildId(TChildId childId)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var sqlBase = Sql.Builder
                       .Where($"{ChildIdLabel} = @0", childId);
                scope.Database.Delete<TJoinDBModel>(sqlBase);
                scope.Complete();
            }
        }

        public bool ExistsParentId(TParentId parentId)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                var sqlBase = Sql.Builder
                       .Where($"{ParentIdLabel} = @0", parentId);
                return scope.Database.FirstOrDefault<TJoinDBModel>(sqlBase) != null;
            }
        }

        public bool ExistsChildId(TChildId childId)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                var sqlBase = Sql.Builder
                       .Where($"{ChildIdLabel} = @0", childId);
                return scope.Database.FirstOrDefault<TJoinDBModel>(sqlBase) != null;
            }
        }

        protected abstract TParentId GetParentId(TParent parent);
    }
}