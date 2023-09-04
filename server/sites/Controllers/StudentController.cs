using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Mlok.Core.Data;
using Mlok.Core.Utils;
using Mlok.Web.Sites.JobChIN.Models;
using Mlok.Web.Sites.JobChIN.Models.Filters;
using Mlok.Web.Sites.JobChIN.Models.StudentModels;
using UmbracoDatabase = Umbraco.Core.Persistence.Database;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class StudentController : CompositeFilterPagedController<Student, JobChIN_Student, StudentFilter, int>
    {
        private static readonly string[] UPDATE_DATE_COLUMNS = new[] { "LastTimeUpdatedByStudent" };
        private static readonly string[] BASIC_INFO_COLUMNS = new[] { "ActiveDriver", "DrivingLicense", "CareerVision", "Presentation", "PreferedLocationId", "WillingToMove", "PreferredJobBeginning" };
        private static readonly string[] WORK_EXPERIENCES_COLUMNS = new[] { "CareerPortfolio" };
        private static readonly string[] CONTACT_COLUMNS = new[] { "PrivateEmail", "Phone", "Linkedin", "Facebook", "Twitter" };
        private static readonly string[] STUDIES_COLUMNS = new[] { "AdditionalEducation" };
        private static readonly string[] NOTIFICATION_COLUMNS = new[] { "NotificationFrequency", "NotificationEmail", "WorkPositionNotificationFrequency" };
        private static readonly string[] VISIBILITY_COLUMNS = new[] { "ProvideContact" };

        public StudentController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        public override Expression<Func<Student, int>> GetModelId() => student => student.StudentId;

        protected override DataProviderSql<JobChIN_Student> GetDataProvider(UmbracoDatabase database, StudentFilter filter)
        {
            bool findUco = filter.Uco.HasValue;
            bool findFirstname = !string.IsNullOrWhiteSpace(filter.Firstname);
            bool findSurname = !string.IsNullOrWhiteSpace(filter.Surname);

            var provider = JobChIN_Student.SelectFromDB(database)
                .LeftJoin<Osoby>(oProvider => 
                {
                    var personProvider = oProvider.On((s, o) => s.Uco == o.UCO);
                    if (findUco)
                        personProvider.Where(o => o.UCO == filter.Uco.Value);
                    if (findFirstname)
                        personProvider.Where(o => o.Jmeno.Contains(filter.Firstname));
                    if (findSurname)
                        personProvider.Where(o => o.Prijmeni.Contains(filter.Surname));
                })
                .LeftJoin<JobChIN_StudentFile>(fProvider => fProvider.On((s, f) => s.StudentId == f.StudentId)
                    .Where(x => x.Category == (int)FileCategory.Photo));

            if (filter.Active)
                provider.Where(x => x.Uco > 0);
            else
                provider.Where(x => x.Uco == 0);

            if (filter.AreaOfInterests?.Any() ?? false)
                provider.InnerJoin<JobChIN_StudentAreaOfInterest>(aoiProvider => aoiProvider.On((s, os) => s.StudentId == os.StudentId)
                    .Where(x => x.AreaOfInterestId, SqlCompareType.In, filter.AreaOfInterests));

            if (filter.HardSkills?.Any() ?? false)
                provider.InnerJoin<JobChIN_StudentHardSkill>(hsProvider => hsProvider.On((s, os) => s.StudentId == os.StudentId)
                    .Where(x => x.HardSkillId, SqlCompareType.In, filter.HardSkills));

            if (filter.WorkPositionId.HasValue)
                provider.InnerJoin<JobChIN_StudentShownInterest>(ssiProvider => ssiProvider.On((s, os) => s.StudentId == os.StudentId)
                    .Where(x => x.WorkPositionId == filter.WorkPositionId.Value));

            if (filter.CompanyId.HasValue)
            {
                provider.LeftJoin<JobChIN_CompanyStudentRevealed>(csrProvider => csrProvider.On((s, os) => s.StudentId == os.StudentId)
                        .Where(x => x.CompanyId == filter.CompanyId.Value))
                    .LeftJoin<JobChIN_StudentShownInterest>(ssiProvider => ssiProvider.On((s, os) => s.StudentId == os.StudentId)
                        .LeftJoin<JobChIN_WorkPosition>(wpProvider => wpProvider.On((ssi, wp) => ssi.WorkPositionId == wp.WorkPositionId)
                            .Where(wp => wp.CompanyId == filter.CompanyId.Value)));
            }

            if (filter.WorkPositionId.HasValue || filter.HiddenWorkPositionId.HasValue)
            {
                var wpId = filter.WorkPositionId.HasValue ? filter.WorkPositionId.Value : filter.HiddenWorkPositionId.Value;
                provider.InnerJoin<JobChIN_Match>(p =>
                    p.On((s, m) => s.StudentId == m.StudentId)
                        .Where(m => m.WorkPositionId == wpId));

                provider.OrderByDesc<JobChIN_Match>(x => x.Suitable)
                    .OrderByDesc<JobChIN_Match>(x => x.OverallMatch);
            }

            return provider;
        }

        protected override DataProviderSql<JobChIN_Student> GetDataProvider(UmbracoDatabase database) => JobChIN_Student.SelectFromDB(database)
             .LeftJoin<Osoby>(oProvider => oProvider.On((s, o) => s.Uco == o.UCO)
                .LeftJoin<Studijni_Pomery>(pomeryProvider => pomeryProvider.On((o, sp) => o.Osobni_Cislo == sp.Osobni_Cislo)
                    .Where(x => x.Aktivni || x.Uspesne_Ukonceno)
                    .LeftJoin<Pracoviste>(pracovisteProvider => pracovisteProvider.On((sp, p) => sp.Fakulta == p.Cislo_Pracoviste))
                    .LeftJoin<Studium_Programy>(programProvider => programProvider.On((sp, prog) => sp.Program == prog.ID_Program))
                    .LeftJoin<Studijni_Pomery_Obory>(spoProvider => spoProvider.On((sp, spo) => sp.ID == spo.Studijni_Pomer_ID)
                        .LeftJoin<Studium_Obory>(oboryProvider => oboryProvider.On((spo, obory) => spo.Obor == obory.Kod_Obor)))))
            .LeftJoin<JobChIN_StudentOtherStudy>(provider => provider.On((s, os) => s.StudentId == os.StudentId))
            .LeftJoin<JobChIN_StudentHardSkill>(provider => provider.On((s, os) => s.StudentId == os.StudentId))
            .LeftJoin<JobChIN_StudentSoftSkill>(provider => provider.On((s, os) => s.StudentId == os.StudentId))
            .LeftJoin<JobChIN_StudentLanguageSkill>(provider => provider.On((s, os) => s.StudentId == os.StudentId))
            .LeftJoin<JobChIN_StudentAreaOfInterest>(provider => provider.On((s, os) => s.StudentId == os.StudentId))
            .LeftJoin<JobChIN_StudentPreferredContractType>(provider => provider.On((s, os) => s.StudentId == os.StudentId))
            .LeftJoin<JobChIN_StudentWorkExperience>(provider => provider.On((s, os) => s.StudentId == os.StudentId))
            .LeftJoin<JobChIN_StudentFile>(provider => provider.On((s, os) => s.StudentId == os.StudentId));

        public Student GetStudentByMemberId(int memberId)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                var dbModel = GetDataProvider(scope.Database, new StudentFilter())
                    .Where(x => x.MemberId == memberId)
                    .SingleOrDefault();

                return Mapper.Map<Student>(dbModel);
            }
        }

        public override Student GetById(int id) => GetById(new Student() { StudentId = id });

        public Student GetById(Student model)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                var dbModel = GetDataProvider(scope.Database)
                    .Where(x => x.StudentId == model.StudentId)
                    .SingleOrDefault();

                return Mapper.Map(dbModel, model);
            }
        }

        public int? GetUco(int studentId) => JobChIN_Student.SelectFromDB().Where(x => x.StudentId == studentId).SingleOrDefault()?.Uco;
        public int? GetMemberId(int studentId) => JobChIN_Student.SelectFromDB().Where(x => x.StudentId == studentId).SingleOrDefault()?.MemberId;

        protected override IEnumerable<string> GetUpdateColumns(Student model)
        {
            IEnumerable<string> result = Enumerable.Empty<string>();
            if (model.BasicInfo != null || model.WorkExperiences != null || model.Contact != null || model.Studies != null || model.Photo != null)
                result = result.Concat(UPDATE_DATE_COLUMNS);
            if (model.BasicInfo != null)
                result = result.Concat(BASIC_INFO_COLUMNS);
            if (model.WorkExperiences != null)
                result = result.Concat(WORK_EXPERIENCES_COLUMNS);
            if (model.Contact != null)
                result = result.Concat(CONTACT_COLUMNS);
            if (model.Studies != null)
                result = result.Concat(STUDIES_COLUMNS);
            if (model.NotificationSettings != null)
                result = result.Concat(NOTIFICATION_COLUMNS);
            if (model.Visibility != null)
                result = result.Concat(VISIBILITY_COLUMNS);

            return result;
        }
    }
}