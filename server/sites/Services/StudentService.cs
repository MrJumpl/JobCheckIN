using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mlok.Core.Data;
using Mlok.Core.Models;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Controllers;
using Mlok.Web.Sites.JobChIN.Members;
using Mlok.Web.Sites.JobChIN.Models;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Mlok.Web.Sites.JobChIN.Models.StudentModels;
using Umbraco.Core;
using Umbraco.Web.PublishedCache;

namespace Mlok.Web.Sites.JobChIN.Services
{
    public class StudentService : UserService<Student>
    {
        private readonly DbScopeProvider scopeProvider;
        private readonly ISettings settings;
        private readonly StudentFileService fileService;
        private readonly StudentAreaOfInterestController studentAreaOfInterestController;
        private readonly StudentLanguageSkillController studentLanguageSkillController;
        private readonly StudentHardSkillController studentHardSkillController;
        private readonly StudentSoftSkillController studentSoftSkillController;
        private readonly StudentPreferredContractTypeController studentPreferredContractTypeController;
        private readonly StudentOtherStudyController studentOtherStudyController;
        private readonly StudentWorkExperienceController studentWorkExperienceController;
        private readonly StudentShownInterestContorller studentShownInterestContorller;
        private readonly StudentWorkPositionFollowedController studentWorkPositionFollowedController;

        public StudentController StudentController { get; }

        protected override bool IsStudent => true;

        protected override string ClassIdentifier => "Student";

        public StudentService(DbScopeProvider scopeProvider, JobChINMembersPlugin membersPlugin, ISettings settings, StudentFileService fileService) : base(membersPlugin)
        {
            this.scopeProvider = scopeProvider;
            this.settings = settings;
            this.fileService = fileService;
            studentAreaOfInterestController = new StudentAreaOfInterestController(scopeProvider);
            studentLanguageSkillController = new StudentLanguageSkillController(scopeProvider);
            studentHardSkillController = new StudentHardSkillController(scopeProvider);
            studentSoftSkillController = new StudentSoftSkillController(scopeProvider);
            studentPreferredContractTypeController = new StudentPreferredContractTypeController(scopeProvider);
            studentOtherStudyController = new StudentOtherStudyController(scopeProvider);
            studentWorkExperienceController = new StudentWorkExperienceController(scopeProvider);
            studentShownInterestContorller = new StudentShownInterestContorller(scopeProvider);
            studentWorkPositionFollowedController = new StudentWorkPositionFollowedController(scopeProvider);

            StudentController = new StudentController(scopeProvider);
        }

        /// <summary>
        /// Register the student. 
        /// </summary>
        /// <param name="model">Student model to create.</param>
        public Student Register(StudentCreateDto model)
        {
            MemberPublishedContent member = membersPlugin.RegisterExternal(model.ExternalLoginProvider);

            Student student = new Student();
            student.Member = member;
            student.ProvideContact = model.ProvideContact;
            student.Uco = membersPlugin.GetCurrentUserUco();
            student.AgreedTo = settings.StudentAgreedTo;
            fileService.CreateFolder(student);
            student.NotificationSettings = new NotificationSettings()
            {
                NotificationFrequency = NotificationFrequency.Immediately,
                WorkPositionNotificationFrequency = NotificationFrequency.Immediately,
            };

            StudentController.Update(student);
            membersPlugin.LoginMember(student.Member);

            return student;
        }

        /// <summary>
        /// Updates the student current state. Only properties that are not null, will be updated.
        /// </summary>
        /// <param name="model">Student model to udpate.</param>
        public Result<StudentUpdateDto> UpdateStudent(StudentUpdateDto model)
        {
            Student student = Mapper.Map<Student>(model);
            var oldStudent = GetCurrentDetailed();
            student.StudentId = oldStudent.StudentId;
            student.MediaFolderId = oldStudent.MediaFolderId;
            student.LastTimeUpdatedByStudent = DateTime.Now.Date;
            student = Update(student, oldStudent);
            return new Result<StudentUpdateDto>(Mapper.Map<StudentUpdateDto>(student));
        }

        /// <summary>
        /// Updates student model also with joined data. Only properties that are not null, will be updated.
        /// </summary>
        /// <param name="model">New state to update.</param>
        /// <param name="oldModel">Current state of the student in database. If it is null, then it is get from database.</param>
        public Student Update(Student student, Student oldModel = null)
        {
            if (oldModel == null)
            {
                oldModel = StudentController.GetById(student.StudentId);
                student.LastTimeUpdatedByStudent = oldModel.LastTimeUpdatedByStudent;
            }

            student = StudentController.Update(student);
            UpdateJoinedData(student, oldModel);

            return student;
        }

        /// <summary>
        /// Returns if the student shown interest on given work position.
        /// </summary>
        public bool HasShownInterest(int workPositionId, int studentId) => studentShownInterestContorller.HasShownInterest(workPositionId, studentId);

        public Result FavoriteWorkPosition(int workPositionId, bool active)
        {
            var student = GetCurrent();
            if (active)
                studentWorkPositionFollowedController.Add(student.StudentId, workPositionId);
            else 
                studentWorkPositionFollowedController.Remove(student.StudentId, workPositionId);
            return new Result();
        }

        /// <summary>
        /// Returns if the given work position is the student's favorite.
        /// </summary>
        public bool HasFavoriteWorkPosition(int workPositionId, int studentId) => studentWorkPositionFollowedController.HasFollow(studentId, workPositionId);

        /// <summary>
        /// Retuirn detailed student model from databse.
        /// </summary>
        public Student GetCurrentDetailed(int? memberId = null)
        {
            var student = GetCurrent(memberId);
            return StudentController.GetById(student.StudentId);
        }

        protected override Student GetByMemberId(int memberId) => StudentController.GetStudentByMemberId(memberId);

        public override void DeleteUserByMemberId(int memberId)
        {
            var model = StudentController.GetStudentByMemberId(memberId);
            fileService.DeleteFolder(model);
            model.AnonymizeData();
            StudentController.CompositeUpdate(model);
        }

        protected override IEnumerable<int> GetMemberIds(int id) => StudentController.GetMemberId(id)?.AsEnumerableOfOne() ?? Enumerable.Empty<int>();

        /// <summary>
        /// Updates joined data. Only properties that are not null, will be updated.
        /// </summary>
        void UpdateJoinedData(Student student, Student oldModel)
        {
            if (student.Studies != null)
                student.Studies.Others = studentOtherStudyController.Update(student.StudentId, student.Studies.Others);
            if (student.Skills != null)
            {
                studentHardSkillController.Join(student);
                studentLanguageSkillController.Join(student);
                studentSoftSkillController.Join(student);
                fileService.UpdateFiles(FileCategory.EducationCertificate | FileCategory.LanguageCertificate, student, oldModel);
            }
            if (student.BasicInfo != null)
            {
                studentAreaOfInterestController.Join(student);
                studentPreferredContractTypeController.Join(student);
            }
            if (student.Photo != null)
                fileService.UpdateFiles(FileCategory.Photo, student, oldModel);
            if (student.WorkExperiences != null)
                student.WorkExperiences.Experiences = studentWorkExperienceController.Update(student.StudentId, student.WorkExperiences.Experiences);
        }
    }
}