using AutoMapper;
using Mlok.Core.Data;
using Mlok.Umbraco;
using Mlok.Web.Sites.JobChIN.Models.StudentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Core.Persistence;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class StudentOtherStudyController : MultiJoinController<int, OtherStudy, JobChIN_StudentOtherStudy>
    {
        public StudentOtherStudyController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override IEnumerable<JobChIN_StudentOtherStudy> GetOldValues(UmbracoDatabase database, int parentId)
        {
            return JobChIN_StudentOtherStudy.SelectFromDB(database)
                .Where(x => x.StudentId == parentId)
                .Execute()
                .ToList();
        }

        protected override bool SameId(OtherStudy model, JobChIN_StudentOtherStudy dbModel)
        {
            return model.OtherUniversityStudyId == dbModel.OtherUniversityStudyId;
        }

        public override Expression<Func<OtherStudy, int>> GetModelId() => otherStudy => otherStudy.OtherUniversityStudyId;

        protected override JobChIN_StudentOtherStudy GetDBModel(int parentId)
        {
            return new JobChIN_StudentOtherStudy() { StudentId = parentId };
        }
    }
}