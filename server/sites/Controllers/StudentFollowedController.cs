using Mlok.Core.Data;
using Umbraco.Core.Persistence;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public abstract class StudentFollowedController<TDBModel>
        where TDBModel : DataResult
    {
        protected DbScopeProvider ScopeProvider { get; }

        protected StudentFollowedController(DbScopeProvider scopeProvider)
        {
            ScopeProvider = scopeProvider;
        }

        public bool HasFollow(int studentId, int followedId) => GetFollowedModel(studentId, followedId) != null;

        public void Add(int studentId, int followedId)
        {
            var model = GetFollowedModel(studentId, followedId);

            using (var scope = ScopeProvider.CreateScope())
            {
                bool insert = !HasFollow(studentId, followedId);
                if (insert)
                    scope.Database.Insert(model);
                scope.Complete();
            }
        }

        public void Remove(int studentId, int followedId)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var sqlBase = Sql.Builder
                       .Where($"StudentId = @0", studentId)
                       .Where($"{FollowedIdLabel} = @0", followedId);
                scope.Database.Delete<TDBModel>(sqlBase);
                scope.Complete();
            }
        }
        
        protected abstract string FollowedIdLabel { get; }
        protected abstract TDBModel GetFollowedModel(int studentId, int followedId);
    }
}