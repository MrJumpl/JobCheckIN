using Mlok.Core.Data;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class StudentWorkPositionViewedController
    {
        protected DbScopeProvider ScopeProvider { get; }

        public StudentWorkPositionViewedController(DbScopeProvider scopeProvider)
        {
            ScopeProvider = scopeProvider;
        }

        public void Add(int studentId, int workPositionId)
        {
            var model = new JobChIN_StudentWorkPositionViewed()
            {
                StudentId = studentId,
                WorkPositionId = workPositionId,
            };

            using (var scope = ScopeProvider.CreateScope())
            {
                bool insert = JobChIN_StudentWorkPositionViewed.SelectFromDB(scope.Database).Where(x => x.StudentId == studentId)
                    .Where(x => x.WorkPositionId == workPositionId)
                    .SingleOrDefault() == null;
                if (insert)
                    scope.Database.Insert(model);
                scope.Complete();
            }
        }
    }
}