using Mlok.Core.Data;
using Mlok.Web.Sites.JobChIN.Controllers;
using Mlok.Web.Sites.JobChIN.Members;
using System.Threading.Tasks;

namespace Mlok.Web.Sites.JobChIN.Services
{
    public class StudentPhotoService
    {
        private readonly JobChINMembersPlugin membersPlugin;
        private readonly StudentService studentService;
        private readonly CompanyService companyService;

        private readonly StudentPhotoController studentPhotoController;
        private readonly StudentController studentController;
        private readonly CompanyStudentRevealedController companyStudentRevealedController;
        private readonly StudentShownInterestContorller studentShownInterestContorller;

        public StudentPhotoService(DbScopeProvider scopeProvider, JobChINMembersPlugin  membersPlugin, StudentService studentService, CompanyService companyService)
        {
            this.membersPlugin = membersPlugin;
            this.studentService = studentService;
            this.companyService = companyService;
            studentPhotoController = new StudentPhotoController(scopeProvider);
            studentController = new StudentController(scopeProvider);
            companyStudentRevealedController = new CompanyStudentRevealedController(scopeProvider);
            studentShownInterestContorller = new StudentShownInterestContorller(scopeProvider);
        }

        /// <summary>
        /// Check if the current logged in member can see the student photo. If not then returns default no photo.
        /// </summary>
        /// <param name="studentId">Id of the student to get photo.</param>
        public async Task<byte[]> GetPhoto(int studentId)
        {
            if (membersPlugin.GetCurrentMember() ==  null)
                return await studentPhotoController.GetNoPhoto();

            var student = studentService.GetCurrent();
            if (student != null)
            {
                if (student.StudentId == studentId)
                    return await studentPhotoController.GetPhoto(student.Uco);
                else
                    return await studentPhotoController.GetNoPhoto();
            }

            var company = companyService.GetCurrent();
            if (company != null && 
                (companyStudentRevealedController.HasRevealedStudent(company.CompanyId, studentId) || studentShownInterestContorller.CompanyHasAccess(company.CompanyId, studentId)))
            {
                var uco = studentController.GetUco(studentId);
                if (uco.HasValue)
                    return await studentPhotoController.GetPhoto(uco.Value);
            }

            return await studentPhotoController.GetNoPhoto();
        }
    }
}