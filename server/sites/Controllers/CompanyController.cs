using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Mlok.Core.Data;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Models;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using Mlok.Web.Sites.JobChIN.Models.Filters;
using Umbraco.Core.Persistence;
using UmbracoDatabase = Umbraco.Core.Persistence.Database;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class CompanyController : CompositeFilterPagedController<Company, JobChIN_Company, CompanyFilter, int>, IPickerController<int?>
    {
        private static readonly string[] GENERAL_INFO_COLUMNS = new[] { "ICO", "DIC", "CompanyName", "City", "Street", "ZipCode", "CountryId", "CorrespondenceCity", "CorrespondenceStreet", "CorrespondenceZip", "CorrespondenceCountryId", "Sector", "WorkerCountRangeId" };
        private static readonly string[] PRESERNTATION_COLUMNS = new[] { "Web", "Linkedin", "Facebook", "ShortDescription", "Description", "Differences", "InterviewDescription" };
        private static readonly string[] CANDIDATES_COLUMNS = new[] { "PeopleTypesSought" };
        private static readonly string[] CONFIRMED_COLUMNS = new[] { "Confirmed" };

        public CompanyController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        public int ConfirmCompany(IEnumerable<int> ids)
        {
            int result = 0;
            using (var scope = ScopeProvider.CreateScope())
            {
                foreach (var id in ids)
                    result += scope.Database.Update(new JobChIN_Company() { CompanyId = id, Confirmed = true }, CONFIRMED_COLUMNS);
                scope.Complete();
            }
            return result;
        }

        public override Expression<Func<Company, int>> GetModelId() => company => company.CompanyId;

        public override Company GetById(int id) => GetById(new Company() { CompanyId = id });

        public Company GetById(Company model)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                var dbModel = GetDataProvider(scope.Database)
                    .Where(x => x.CompanyId == model.CompanyId)
                    .SingleOrDefault();

                return Mapper.Map(dbModel, model);
            }
        }

        public IEnumerable<EnumerablePickerValue<int?, string>> GetPicker() => JobChIN_Company.SelectFromDB()
            .Where(x => x.Confirmed)
            .Execute()
            .Select(x => EnumerablePickerValue.From<int?, string>(x.CompanyId, x.CompanyName))
            .ToList(); 

        protected override DataProviderSql<JobChIN_Company> GetDataProvider(UmbracoDatabase database, CompanyFilter filter)
        {
            var provider = JobChIN_Company.SelectFromDB(database)
                .Where(x => x.Archived == filter.Archived);
            if (filter.IsConfirmed.HasValue)
                provider.Where(x => x.Confirmed == filter.IsConfirmed.Value);
            if (filter.Active)
                provider.InnerJoin<JobChIN_CompanyCompanyType>(joinProvider => joinProvider
                    .On((company, join) => company.CompanyId == join.CompanyId)
                    .Where(x => x.Confirmed || x.Paid)
                    .Where(x => x.ActiveFrom <= DateTime.Now)
                    .Where(x => x.ActiveTo > DateTime.Now)
                    .Where(x => x.DatabaseSearch));
            provider.Where(x => x.Archived == filter.Archived);
            if (filter.IncludeLogo)
                provider.LeftJoin<JobChIN_CompanyFile>(joinProvider => joinProvider
                    .On((company, join) => company.CompanyId == join.CompanyId)
                    .Where(x => x.Category == (int)FileCategory.Logo));
            if (!string.IsNullOrWhiteSpace(filter.CompanyName))
                provider.Where(x => x.CompanyName.Contains(filter.CompanyName));
            if (filter.AreaOfInterests?.Any() ?? false)
                provider.InnerJoin<JobChIN_CompanyAreaOfInterest>(joinProvider => joinProvider
                    .On((company, join) => company.CompanyId == join.CompanyId)
                    .Where(x => x.AreaOfInterestId, SqlCompareType.In, filter.AreaOfInterests));
            if (filter.IncludeWorkPositionStats)
                provider.LeftJoin<JobChIN_WorkPosition>(joinProvider => joinProvider
                    .On((company, join) => company.CompanyId == join.CompanyId)
                    .Where(x => x.Publication <= DateTime.Now)
                    .Where(x => x.Expiration > DateTime.Now));
            if (!string.IsNullOrWhiteSpace(filter.Faculty))
                provider.InnerJoin<JobChIN_CompanyFaculty>(joinProvider => joinProvider
                    .On((company, join) => company.CompanyId == join.CompanyId)
                    .Where(x => x.FacultyId == filter.Faculty));
            if (filter.OnlyHasWorkPosition)
                provider.InnerJoin<JobChIN_WorkPosition>(joinProvider => joinProvider
                    .On((company, join) => company.CompanyId == join.CompanyId)
                    .Where(x => x.Publication >= DateTime.Now && x.Expiration < DateTime.Now));
            if (filter.Sector.HasValue)
                provider.Where(x => x.Sector == (int)filter.Sector);
            if (!string.IsNullOrWhiteSpace(filter.Ico))
                provider.Where(x => x.ICO == filter.Ico);

            return provider;
        }

        protected override DataProviderSql<JobChIN_Company> GetDataProvider(UmbracoDatabase database) => JobChIN_Company.SelectFromDB(database)
                .LeftJoin<JobChIN_CompanyUser>(provider => provider.On((company, join) => company.CompanyId == join.CompanyId))
                .LeftJoin<JobChIN_CompanyAreaOfInterest>(provider => provider.On((company, join) => company.CompanyId == join.CompanyId))
                .LeftJoin<JobChIN_CompanyFaculty>(provider => provider.On((company, join) => company.CompanyId == join.CompanyId))
                .LeftJoin<JobChIN_CompanyLanguageSkillPreferred>(provider => provider.On((company, join) => company.CompanyId == join.CompanyId))
                .LeftJoin<JobChIN_CompanyCompanyType>(provider => provider.OnUsingNewQuery((company, companyType) => company.CompanyId == companyType.CompanyId))
                .LeftJoin<JobChIN_CompanyFile>(provider => provider.On((company, join) => company.CompanyId == join.CompanyId))
                .LeftJoin<JobChIN_CompanyBranch>(provider => provider.On((company, join) => company.CompanyId == join.CompanyId));

        protected override Company Insert(Company model)
        {
            model.Created = DateTime.Now;
            return base.Insert(model);
        }

        protected override IEnumerable<string> GetUpdateColumns(Company model)
        {
            var result = Enumerable.Empty<string>();
            if (model.GeneralInfo != null)
                result = result.Concat(GENERAL_INFO_COLUMNS);
            if (model.Presentation != null)
                result = result.Concat(PRESERNTATION_COLUMNS);
            if (model.Candidates != null)
                result = result.Concat(CANDIDATES_COLUMNS);

            return result;
        }

        public override void Delete(IEnumerable<int> ids)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var sql = Sql.Builder.Where("CompanyId IN (@0)", ids);
                scope.Database.Delete<JobChIN_CompanyAreaOfInterest>(sql);
                scope.Database.Delete<JobChIN_CompanyBranch>(sql);
                scope.Database.Delete<JobChIN_CompanyCompanyType>(sql);
                scope.Database.Delete<JobChIN_CompanyFaculty>(sql);
                scope.Database.Delete<JobChIN_CompanyFile>(sql);
                scope.Database.Delete<JobChIN_CompanyLanguageSkillPreferred>(sql);
                scope.Database.Delete<JobChIN_CompanyStudentRevealed>(sql);
                scope.Complete();

                base.Delete(ids);
            }
        }
    }

    public class CompanyUserController
    {
        private static readonly string[] CONTACT_PERSON_COLUMNS = new[] { "Firstname", "Surname", "Phone" };
        private static readonly string[] NOTIFICATION_COLUMNS = new[] { "NotificationFrequency", "NotificationEmail" };
        private static readonly string[] ROLE_COLUMNS = new[] { "Role" };

        private readonly DbScopeProvider scopeProvider;

        public CompanyUserController(DbScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        public CompanyUser GetByMemberId(int memberId)
        {
            using (var scope = scopeProvider.CreateReadOnlyScope())
            {
                var dbModel = JobChIN_CompanyUser.SelectFromDB()
                    .Where(user => user.MemberId == memberId)
                    .SingleOrDefault();
                return Mapper.Map<CompanyUser>(dbModel);
            }
        }

        public CompanyUser Register(CompanyUser user)  
        {
            using (var scope = scopeProvider.CreateScope())
            {
                scope.Database.Insert(Mapper.Map<JobChIN_CompanyUser>(user));
                scope.Complete();
            }

            return user;
        }

        public CompanyUser Update(CompanyUser user)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var columns = GetUpdateColumns(user);
                if (columns.Any())
                {
                    var dbModel = Mapper.Map<JobChIN_CompanyUser>(user);
                    scope.Database.Update(dbModel, columns);
                    user = Mapper.Map<CompanyUser>(dbModel);
                }

                scope.Complete();
            }

            return user;
        }

        public CompanyUser UpdateRole(CompanyUser user)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                scope.Database.Update(Mapper.Map<JobChIN_CompanyUser>(user), ROLE_COLUMNS);
                scope.Complete();
            }

            return user;
        }

        public CompanyUser CompositeUpdate(CompanyUser user)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var dbModel = Mapper.Map<JobChIN_CompanyUser>(user);
                scope.Database.Update(dbModel, ROLE_COLUMNS.Concat(CONTACT_PERSON_COLUMNS).Concat(NOTIFICATION_COLUMNS));
                user = Mapper.Map<CompanyUser>(dbModel);
                scope.Complete();
            }

            return user;
        }

        public void Delete(int memberId)
        {
            using (var scope = scopeProvider.CreateScope())
            {
                var sql = Sql.Builder.Where("CompanyUserId = @0", memberId);
                scope.Database.Delete<JobChIN_WorkPositionUser>(sql);

                scope.Database.Delete<JobChIN_CompanyUser>(memberId);
                scope.Complete();
            }
        }

        public IEnumerable<int> GetMemberIdsByCompanyId(int companyId)
        {
            using (var scope = scopeProvider.CreateReadOnlyScope())
            {
                return JobChIN_CompanyUser.SelectFromDB(scope.Database)
                    .Where(x => x.CompanyId == companyId)
                    .Execute()
                    .Select(x => x.MemberId);
            }
        }

        IEnumerable<string> GetUpdateColumns(CompanyUser model)
        {
            var result = Enumerable.Empty<string>();
            if (model.ContactPerson != null)
                result = result.Concat(CONTACT_PERSON_COLUMNS);
            if (model.NotificationSettings != null)
                result = result.Concat(NOTIFICATION_COLUMNS);

            return result;
        }
    }
}