using Mlok.Core.Data;
using System;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class CompanyStudentRevealedController
    {
        protected DbScopeProvider ScopeProvider { get; }

        public CompanyStudentRevealedController(DbScopeProvider scopeProvider)
        {
            ScopeProvider = scopeProvider;
        }

        public void Add(int companyId, int studentId)
        {
            var model = new JobChIN_CompanyStudentRevealed()
            {
                CompanyId = companyId,
                StudentId = studentId,
                Date = DateTime.Now,
            };

            using (var scope = ScopeProvider.CreateScope())
            {
                scope.Database.Insert(model);
                scope.Complete();
            }
        }

        public bool HasRevealedStudent(int companyId, int studentId)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                return JobChIN_CompanyStudentRevealed.SelectFromDB(scope.Database)
                    .Where(x => x.CompanyId == companyId)
                    .Where(x => x.StudentId == studentId)
                    .SingleOrDefault() != null;
            }
        }

        public int Count(int companyId, DateTime from)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                return JobChIN_CompanyStudentRevealed.SelectFromDB(scope.Database)
                    .Where(x => x.CompanyId == companyId)
                    .Where(x => x.Date >= from)
                    .Execute()
                    .Count();
            }
        }
    }
}