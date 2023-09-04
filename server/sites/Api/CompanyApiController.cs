using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Api.Controllers;
using Mlok.Web.Sites.JobChIN.Api.Attributes;
using Mlok.Web.Sites.JobChIN.Models;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Mlok.Web.Sites.JobChIN.Api
{
    public class CompanyApiController : WebCentrumAngularApiModuleController<JobChINModule>
    {
        /// <summary>
        /// Validates the ico if it is exists and is not registred in system.
        /// </summary>
        /// <param name="ico">ICO to validate</param>
        [HttpGet]
        public async Task<HttpResponseMessage> ValidateCompanyIco(string ico)
        {
            var result = await Module.CompanyService.ValidateCompanyIco(ico);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Returns the company registration config such as url for GRDP agreement.
        /// </summary>
        [HttpGet]
        public HttpResponseMessage GetRegistrationGonfig()
        {
            return Module.CompanyRegistrationConfig().ToOk(Request);
        }

        /// <summary>
        /// Register the company. 
        /// </summary>
        /// <param name="model">Company model to create.</param>
        [HttpPost]
        public async Task<HttpResponseMessage> RegisterCompany(CompanyCreateDto model)
        {
            var result = await Module.CompanyService.Register(model);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Updates the company current state. Only properties that are not null, will be updated.
        /// </summary>
        /// <param name="model">Company model to udpate.</param>
        [HttpPost]
        [CompanyAuthorize(Role.CompanyAdmin)]
        public HttpResponseMessage UpdateCompany(CompanyUpdateDto model)
        {
            var result = Module.CompanyService.Update(model);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Updates the company user current state. Only properties that are not null, will be updated.
        /// </summary>
        /// <param name="model">Company user model to update.</param>
        [HttpPost]
        [CompanyAuthorize]
        public HttpResponseMessage UpdateUser(CompanyUserUpdateDto model)
        {
            var result = Module.CompanyService.UpdateUser(model);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Send an email notification to the new user with a link where the new user can register to this company.
        /// </summary>
        /// <param name="model">Model that contains the email of new user.</param>
        [HttpPost]
        [CompanyAuthorize(Role.CompanyAdmin)]
        public HttpResponseMessage SendInvitaion(SendUserInvitaion model)
        {
            var companyUser = Module.CompanyService.GetCurrent();
            var result = Module.MembersPlugin.SendNewUserInvitaion(model, companyUser.Member, companyUser.CompanyId);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Register the nwe user to the given company. 
        /// </summary>
        /// <param name="model">New user model.</param>
        [HttpPost]
        public HttpResponseMessage CreateNewUser(NewCompanyUserDto model)
        {
            var result = Module.CompanyService.CreateNewUser(model);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Create the OC request and return the url where to redirect user.
        /// </summary>
        /// <param name="model">Model that represent which package the company wants to buy.</param>
        [HttpPost]
        [CompanyAuthorize(Role.CompanyAdmin)]
        public HttpResponseMessage BuyCompanyType(BuyCompanyTypeDto model)
        {
            var result = Module.CompanyService.BuyCompanyType(model);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Returns the invoice for the purcahsed company type. 
        /// </summary>
        /// <param name="requestId">OC request id.</param>
        [HttpPost]
        [CompanyAuthorize(Role.CompanyAdmin)]
        public IHttpActionResult DownloadInvoice([FromBody]int requestId)
        {
            var result = Module.CompanyService.DownloadInvoice(requestId);
            if (result.ActionResult.ResultType == WebDataActionResultType.Error)
                return Content(System.Net.HttpStatusCode.Forbidden, result.ActionResult.Message);
            return new FileResult(result.Content, result.FileName, MimeMapping.GetMimeMapping(result.FileName), true, new CacheControlHeaderValue() { NoStore = true });
        }

        /// <summary>
        /// Return the current state of work position.
        /// </summary>
        /// <param name="workPositionId">Id of work position.</param>
        [HttpGet]
        [CompanyAuthorize]
        public HttpResponseMessage GetWorkPositionDetail(int workPositionId)
        {
            var companyUser = Module.CompanyService.GetCurrent();
            var result = Module.WorkPositionService.GetCompanyWorkPositionDetail(companyUser, workPositionId);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Return the intervals in which company can create the new work positions.
        /// </summary>
        /// <param name="workPositionId">Id of work position to ignore.</param>
        [HttpGet]
        [CompanyAuthorize]
        public HttpResponseMessage GetCompanyFreeSlots(int? workPositionId = null)
        {
            var companyUser = Module.CompanyService.GetCurrent();
            var result = Module.WorkPositionService.GetCompanyFreeSlots(companyUser.CompanyId, workPositionId);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Create a new work position.
        /// </summary>
        /// <param name="model">State of the work position to create.</param>
        [HttpPost]
        [CompanyAuthorize(Role.CompanyAdmin)]
        public HttpResponseMessage CreateWorkPosition(WorkPositionCreateDto model)
        {
            var companyUser = Module.CompanyService.GetCurrent();
            var result = Module.WorkPositionService.Create(companyUser.CompanyId, model);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Save the state of the work position. Only properties that are not null, will be updated.
        /// </summary>
        /// <param name="model">State of the work position to save.</param>
        [HttpPost]
        [CompanyAuthorize]
        public HttpResponseMessage UpdateWorkPosition(WorkPositionUpdateDto model)
        {
            var companyUser = Module.CompanyService.GetCurrent();
            var result = Module.WorkPositionService.Update(companyUser, model);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Return the preview of the work position on the given model. The preview is the same as the one the student will see after the publication.
        /// </summary>
        /// <param name="model">State of the work position to display.</param>
        [HttpPost]
        [CompanyAuthorize]
        public HttpResponseMessage GetWorkPositionPreview(WorkPositionUpdateDto model)
        {
            var company = Module.CompanyService.GetCurrentCompany();
            var result = Module.WorkPositionService.GetWorkPositionDetail(company, model);
            return result.ToOk(Request);
        }

        /// <summary>
        ///  Returns the page of work positions that expiry date is greater than now.
        /// </summary>
        /// <param name="page">Page number to display.</param>
        [HttpGet]
        [CompanyAuthorize]
        public HttpResponseMessage GetCurrentWorkPositionPaged(int page)
        {
            var company = Module.CompanyService.GetCurrent();
            var result = Module.WorkPositionService.GetCompanyWorkPositionsPaged(company, page, true);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Returns the page of work positions that expiry date is less than now.
        /// </summary>
        /// <param name="page">Page number to display.</param>
        [HttpGet]
        [CompanyAuthorize]
        public HttpResponseMessage GetArchivedWorkPositionPaged(int page)
        {
            var company = Module.CompanyService.GetCurrent();
            var result = Module.WorkPositionService.GetCompanyWorkPositionsPaged(company, page, false);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Returns the page of students that shown interest for given work position.
        /// </summary>
        /// <param name="workPositionId">Id of the work posiotion.</param>
        /// <param name="page">Page number to display.</param>
        [HttpGet]
        [CompanyAuthorize]
        public HttpResponseMessage GetWorkPositionStudents(int workPositionId, int page)
        {
            var company = Module.CompanyService.GetCurrent();
            var result = Module.WorkPositionService.GetWorkPositionStudents(company, workPositionId, page);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Deletes the user by its membeId.
        /// </summary>
        [HttpPost]
        [CompanyAuthorize(Role.CompanyAdmin)]
        public HttpResponseMessage DeleteUser([FromBody]int memberId)
        {
            var result = Module.CompanyService.DeleteUser(memberId);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Changes the role of given user.
        /// </summary>
        /// <param name="model">Model that identifies the user and role.</param>
        [HttpPost]
        [CompanyAuthorize(Role.CompanyAdmin)]
        public HttpResponseMessage ChangeRole(ChangeRoleDto model)
        {
            var result = Module.CompanyService.ChangeRole(model);
            return result.ToOk(Request);
        }

        /// <summary>
        /// Returns the page of students sorted by match score.
        /// </summary>
        /// <param name="filter">The filter object representing the match criteria.</param>
        [HttpGet]
        [CompanyAuthorize(Role.CompanyAdmin)]
        public HttpResponseMessage SearchStudentPaged([FromUri]SearchStudentFilterDto filter)
        {
            var company = Module.CompanyService.GetCurrent();
            var result = Module.WorkPositionService.SearchStudentPaged(company, filter);
            return result.ToOk(Request);
        }
    }
}