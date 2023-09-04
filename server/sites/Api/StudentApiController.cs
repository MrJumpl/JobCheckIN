using Mlok.Web.Api.Controllers;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Mlok.Web.Sites.JobChIN.Models;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Net.Http.Headers;
using Mlok.Web.Sites.JobChIN.Api.Attributes;

namespace Mlok.Web.Sites.JobChIN.Api
{
    public class StudentApiController : WebCentrumAngularApiModuleController<JobChINModule>
    {
        /// <summary>
        /// Returns the company registration config such as url for GRDP agreement.
        /// </summary>
        [HttpGet]
        public HttpResponseMessage GetRegistrationGonfig()
        {
            return Module.StudentRegistrationConfig().ToOk(Request);
        }

        /// <summary>
        /// Returns the page of work positions that match the filter.
        /// </summary>
        /// <param name="filter">Model to be filtered by.</param>
        [HttpGet]
        [StudentAuthorize]
        public HttpResponseMessage GetWorkPositionsPaged([FromUri]WorkPositionFilterDto filter)
        {
            var student = Module.StudentService.GetCurrent();
            return Module.WorkPositionService.GetStudentWorkPositionsPaged(filter, student.StudentId).ToOk(Request);
        }

        /// <summary>
        /// Rerturns the docx cv by current state of the student profile.
        /// </summary>
        [HttpGet]
        [StudentAuthorize]
        public HttpResponseMessage DownloadDocxCv()
        {
            var student = Module.StudentService.GetCurrentDetailed();
            var model = Module.CvService.GetDocxCv(student);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(model.Content);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = $"cv.{model.FileExt}"
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(model.MimeType);

            return response;
        }

        /// <summary>
        /// Returns the companies that match the filter.
        /// </summary>
        /// <param name="filter">Model to be filtered by.</param>
        [HttpGet]
        [StudentAuthorize]
        public HttpResponseMessage GetCompanies([FromUri]CompanyFilterDto filter)
        {
            return Module.CompanyService.GetActiveCompanies(filter).ToOk(Request);
        }

        /// <summary>
        /// Register the new student that is logged in by MUNI Unified Login.
        /// </summary>
        /// <param name="model">Student model to create.</param>
        [HttpPost]
        public IHttpActionResult RegisterStudent(StudentCreateDto model)
        {
            var student = Module.StudentService.Register(model);
            var response = Module.GetStudentConfig(student);
            return JsonCamelCase(response);
        }

        /// <summary>
        /// Updates the student current state. Only properties that are not null, will be updated.
        /// </summary>
        /// <param name="model">Student mdoel to update.</param>
        [HttpPost]
        [StudentAuthorize]
        public HttpResponseMessage UpdateStudent(StudentUpdateDto model)
        {
            var result = Module.StudentService.UpdateStudent(model);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Show interest to work position. The model has to correspond to the work position.
        /// </summary>
        /// <param name="model">Model that represents the student interest.</param>
        [HttpPost]
        [StudentAuthorize]
        public HttpResponseMessage ShowInterest(ShowInterest model)
        {
            var student = Module.StudentService.GetCurrent();
            var result = Module.WorkPositionService.ShowInterest(student.StudentId, model);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Add or remove the work position from the list of favorite work positions.
        /// </summary>
        /// <param name="workPositionId">Id of work position.</param>
        /// <param name="active">If true then the work position is added to the list of favorite work position.</param>
        [HttpPost]
        [StudentAuthorize]
        public HttpResponseMessage FavoriteWorkPosition(int workPositionId, bool active)
        {
            var result = Module.StudentService.FavoriteWorkPosition(workPositionId, active);
            return result.ToOk(Request);
        }
    }
}