using Mlok.Core.Data;
using Mlok.Web.Sites.JobChIN.Models.StudentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Core.Persistence;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class StudentWorkExperienceController : MultiJoinController<int, WorkExperience, JobChIN_StudentWorkExperience>
    {
        public StudentWorkExperienceController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        public override Expression<Func<WorkExperience, int>> GetModelId() => workExperience => workExperience.WorkExperienceId;

        protected override JobChIN_StudentWorkExperience GetDBModel(int parentId)
        {
            return new JobChIN_StudentWorkExperience() { StudentId = parentId };
        }

        protected override IEnumerable<JobChIN_StudentWorkExperience> GetOldValues(UmbracoDatabase database, int parentId)
        {
            return JobChIN_StudentWorkExperience.SelectFromDB(database)
                .Where(x => x.StudentId == parentId)
                .Execute()
                .ToList();
        }

        protected override bool SameId(WorkExperience model, JobChIN_StudentWorkExperience dbModel)
        {
            return model.WorkExperienceId == dbModel.WorkExperienceId;
        }
    }
}