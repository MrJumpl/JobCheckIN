using Mlok.Umbraco;
using Mlok.Web.Sites.JobChIN.Constants;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace Mlok.Web.Sites.JobChIN.Api
{
    public class StudentPhotoApiController : UmbracoApiController
    {
        private SiteHelper site => SiteHelper.Get(SiteConstants.SiteId);
        private JobChINModule module => site.ResolveModule<JobChINModule>(SiteConstants.ModuleAlias);

        /// <summary>
        /// Returns the default photo of the student from IS.
        /// </summary>
        /// <param name="studentId">Id of the student.</param>
        [HttpGet]
        public async Task<HttpResponseMessage> GetPhoto(int studentId)
        {
            var bytes = await module.StudentPhotoService.GetPhoto(studentId);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(bytes);
            response.Content.Headers.ContentLength = bytes.LongLength;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = new TimeSpan(1, 0, 0, 0),
            };

            return response;
        }
    }
}