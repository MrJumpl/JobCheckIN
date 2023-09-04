using AutoMapper;
using Mlok.Core.Data;
using Mlok.Web.Sites.JobChIN.Models;
using System;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class StudentShownInterestContorller
    {
        protected DbScopeProvider ScopeProvider { get; }

        public StudentShownInterestContorller(DbScopeProvider scopeProvider)
        {
            ScopeProvider = scopeProvider;
        }

        public void Add(int studentId, ShowInterest model)
        {
            var dbModel = new JobChIN_StudentShownInterest()
            {
                StudentId = studentId,
                Date = DateTime.Now,
            };
            dbModel = Mapper.Map(model, dbModel);

            using (var scope = ScopeProvider.CreateScope())
            {
                scope.Database.Insert(dbModel);
                scope.Complete();
            }
        }

        public bool CompanyHasAccess(int companyId, int studentId)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                return JobChIN_StudentShownInterest.SelectFromDB(scope.Database)
                    .Where(x => x.StudentId == studentId)
                    .InnerJoin<JobChIN_WorkPosition>(provider => provider.On((s, wp) => s.WorkPositionId == wp.WorkPositionId)
                        .InnerJoin<JobChIN_Company>(companyProvider => companyProvider.On((wp, c) => wp.CompanyId == c.CompanyId)
                            .Where(x => x.CompanyId == companyId)))
                    .TakeTop(1)
                    .Execute()
                    .FirstOrDefault() != null;
            }
        }

        public bool HasShownInterest(int workPositionId, int studentId)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                return JobChIN_StudentShownInterest.SelectFromDB(scope.Database)
                    .Where(x => x.WorkPositionId == workPositionId)
                    .Where(x => x.StudentId == studentId)
                    .SingleOrDefault() != null;
            }
        }
    }
}