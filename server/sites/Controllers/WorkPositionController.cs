using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Mlok.Core.Data;
using Mlok.Core.Utils;
using Mlok.Web.Sites.JobChIN.Models;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using Mlok.Web.Sites.JobChIN.Models.Filters;
using Umbraco.Core;
using UmbracoDatabase = Umbraco.Core.Persistence.Database;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class WorkPositionController : CompositeFilterPagedController<WorkPosition, JobChIN_WorkPosition, WorkPositionFilter, int>
    {
        private static readonly string[] BASIC_INFO_COLUMNS = new[] { "Name", "Language", "Publication", "Expiration", "JobBeginning", "Remote", "LocationId", "BranchId", "Public" };
        private static readonly string[] DETAIL_COLUMNS = new[] { "CompanyDescription", "Description", "Requesting", "Offering", "CustomField1Name", "CustomField2Name", "CustomField3Name", "CustomField1Value", "CustomField2Value", "CustomField3Value" };
        private static readonly string[] CANDIDATES_COLUMNS = new[] { "HasAreaOfInterest", "HasFaculty", "HasMandatoryLangs", "AreaOfInterestCount", "HardSkillsCount", "SoftSkillsCount", "LanguageOptionalSkillsCount", "DrivingLicense", "ActiveDriver" };
        private static readonly string[] CANDIDATE_REQUEST_COLUMNS = new[] { "CoverLetter", "AdditionalQuestions" };
        private static readonly string[] VISIBILITY_COLUMNS = new[] { "Hidden" };

        public WorkPositionController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override DataProviderSql<JobChIN_WorkPosition> GetDataProvider(UmbracoDatabase database, WorkPositionFilter filter)
        {
            var provider = JobChIN_WorkPosition.SelectFromDB(database);
            if (filter.AreaOfInterests?.Any() ?? false)
                provider.InnerJoin<JobChIN_WorkPositionAreaOfInterest>(p => p.On((wp, aoi) => wp.WorkPositionId == aoi.WorkPositionId)
                    .Where(x => x.AreaOfInterestId, SqlCompareType.In, filter.AreaOfInterests));
            if (filter.ContractTypes?.Any() ?? false)
                provider.InnerJoin<JobChIN_WorkPositionContractType>(p => p.On((wp, aoi) => wp.WorkPositionId == aoi.WorkPositionId)
                    .Where(x => x.ContractTypeId, SqlCompareType.In, filter.ContractTypes));
            if (!filter.Name.IsNullOrWhiteSpace())
                provider.Where(x => x.Name.Contains(filter.Name));
            if (filter.Remotes?.Any() ?? false)
                provider.Where(x => x.Remote, SqlCompareType.In, filter.Remotes.Select(x => (int)x));
            if (filter.Languages?.Any() ?? false)
                provider.InnerJoin<JobChIN_WorkPositionLanguageSkill>(p => p.On((wp, ls) => wp.WorkPositionId == ls.WorkPositionId)
                    .Where(x => x.LanguageId, SqlCompareType.In, filter.Languages));
            if (filter.Locations?.Any() ?? false)
                provider.LeftJoin<JobChIN_CompanyBranch>(p => p.On((wp, b) => wp.BranchId == b.BranchId))
                    .WhereGroup(groupProvider => groupProvider
                        .OrWhere(x => x.LocationId, SqlCompareType.In, filter.Locations)
                        .OrWhereJoined<JobChIN_CompanyBranch>(x => x.LocationId, SqlCompareType.In, filter.Locations));

            if (filter.CompanyId.HasValue)
                provider.Where(x => x.CompanyId == filter.CompanyId.Value);
            if (filter.CompanyUserId.HasValue)
                provider.InnerJoin<JobChIN_WorkPositionUser>(p => p.On((wp, u) => wp.WorkPositionId == u.WorkPositionId)
                    .Where(x => x.CompanyUserId == filter.CompanyUserId.Value));
            if (!filter.IncludeHidden)
                provider.Where(x => !x.Hidden);
            if (filter.Hidden.HasValue)
                provider.Where(x => x.Hidden == filter.Hidden.Value);
            if (filter.Active.HasValue && filter.Active.Value)
                provider.Where(x => x.Expiration >= DateTime.Now);
            else if (filter.Active.HasValue)
                provider.Where(x => x.Expiration < DateTime.Now);
            if (filter.Published.HasValue && filter.Published.Value)
                provider.Where(x => x.Publication <= DateTime.Now);

            if (filter.IncludeStats)
                provider
                    .LeftJoin<JobChIN_StudentWorkPositionViewed>(p => p.On((s, os) => s.WorkPositionId == os.WorkPositionId))
                    .LeftJoin<JobChIN_StudentShownInterest>(p => p.On((s, os) => s.WorkPositionId == os.WorkPositionId));

            if (filter.IncludeCompany)
                provider.InnerJoin<JobChIN_Company>(p => p.On((s, c) => s.CompanyId == c.CompanyId)
                        .LeftJoin<JobChIN_CompanyFile>(pp => pp.On((ss, cc) => ss.CompanyId == cc.CompanyId && cc.FileId == (int)FileCategory.Logo)));

            if (filter.IncludeStudentDetails)
            {
                provider.LeftJoin<JobChIN_WorkPositionContractType>(p => p.OnUsingNewQuery((wp, ct) => wp.WorkPositionId == ct.WorkPositionId));
                provider.LeftJoin<JobChIN_CompanyBranch>(p => p.On((wp, b) => wp.BranchId == b.BranchId));
            }
            if (filter.StudentId.HasValue)
                provider.LeftJoin<JobChIN_StudentWorkPositionFollowed>(p => p.On((wp, f) => wp.WorkPositionId == f.WorkPositionId));

            AddOrderBy(provider, filter.StudentId);

            return provider;
        }

        protected override DataProviderSql<JobChIN_WorkPosition> GetDataProvider(UmbracoDatabase database) => JobChIN_WorkPosition.SelectFromDB(database)
                .LeftJoin<JobChIN_WorkPositionAreaOfInterest>(p => p.On((s, os) => s.WorkPositionId == os.WorkPositionId))
                .LeftJoin<JobChIN_WorkPositionContractType>(p => p.On((s, os) => s.WorkPositionId == os.WorkPositionId))
                .LeftJoin<JobChIN_WorkPositionHardSkill>(p => p.On((s, os) => s.WorkPositionId == os.WorkPositionId))
                .LeftJoin<JobChIN_WorkPositionSoftSkill>(p => p.On((s, os) => s.WorkPositionId == os.WorkPositionId))
                .LeftJoin<JobChIN_WorkPositionLanguageSkill>(p => p.On((s, os) => s.WorkPositionId == os.WorkPositionId))
                .LeftJoin<JobChIN_WorkPositionFaculty>(p => p.On((s, os) => s.WorkPositionId == os.WorkPositionId))
                .LeftJoin<JobChIN_WorkPositionUser>(p => p.On((s, os) => s.WorkPositionId == os.WorkPositionId));

        public override Expression<Func<WorkPosition, int>> GetModelId() => workPosition => workPosition.WorkPositionId;

        protected override WorkPosition Insert(WorkPosition model)
        {
            model.Created = DateTime.Now;
            return base.Insert(model);
        }

        public override WorkPosition GetById(int id)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                var provider = GetDataProvider(scope.Database)
                    .Where(workPosition => workPosition.WorkPositionId == id);
                var dbModel = provider.SingleOrDefault();

                return Mapper.Map<WorkPosition>(dbModel);
            }
        }

        public IEnumerable<WorkPosition> GetCurrentByCompanyId(int companyId, DateTime to)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                var provider = JobChIN_WorkPosition.SelectFromDB(scope.Database)
                    .Where(workPosition => workPosition.CompanyId == companyId)
                    .Where(workPosition => workPosition.Expiration >= DateTime.Now)
                    .Where(workPosition => workPosition.Publication <= to)
                    .Where(workPosition => !workPosition.Hidden);

                return provider.Execute().Select(Mapper.Map<WorkPosition>);
            }
        }

        public IEnumerable<WorkPosition> GetByCompanyId(int companyId)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                var provider = GetDataProvider(scope.Database)
                    .Where(workPosition => workPosition.CompanyId == companyId);

                return provider.Execute().Select(Mapper.Map<WorkPosition>)
                    .ToList();
            }
        }

        public bool ValidateWorkPosition(CompanyUser companyUser, int workPositionId)
        {
            var provider = JobChIN_WorkPosition.SelectFromDB()
                .Where(x => x.CompanyId == companyUser.CompanyId)
                .Where(x => x.WorkPositionId == workPositionId);
            if (companyUser.Role != Role.CompanyAdmin)
                provider = provider.InnerJoin<JobChIN_WorkPositionUser>(p => p.On((wp, u) => wp.WorkPositionId == u.WorkPositionId)
                    .Where(x => x.CompanyUserId == companyUser.Member.Id));

            return provider.SingleOrDefault() != null;
        }

        public void UpdateLocation(int branchId, string locationId)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                var workPositions = JobChIN_WorkPosition.SelectFromDB()
                    .Where(x => x.BranchId == branchId)
                    .Execute();
                foreach (var workPosition in workPositions)
                {
                    workPosition.BranchId = null;
                    workPosition.LocationId = locationId;
                    scope.Database.Update(workPosition);
                }
            }
        }

        protected override IEnumerable<string> GetUpdateColumns(WorkPosition model)
        {
            IEnumerable<string> result = new List<string>();
            if (model.BasicInfo != null)
                result = result.Concat(BASIC_INFO_COLUMNS);
            if (model.Detail != null)
                result = result.Concat(DETAIL_COLUMNS);
            if (model.Candidates != null)
                result = result.Concat(CANDIDATES_COLUMNS);
            if (model.CandidateRequest != null)
                result = result.Concat(CANDIDATE_REQUEST_COLUMNS);
            if (model.Visibility != null)
                result = result.Concat(VISIBILITY_COLUMNS);

            return result;
        }

        private void AddOrderBy(DataProviderSql<JobChIN_WorkPosition> provider, int? studentId)
        {
            if (studentId.HasValue)
            {
                provider.InnerJoin<JobChIN_Match>(p =>
                    p.On((wp, m) => wp.WorkPositionId == m.WorkPositionId)
                        .Where(m => m.StudentId == studentId));

                provider.OrderByDesc<JobChIN_Match>(x => x.Suitable)
                    .OrderByDesc<JobChIN_Match>(x => x.OverallMatch);
            }
        }
    }
}