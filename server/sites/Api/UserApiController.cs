using Mlok.Web.Api.Controllers;
using Mlok.Web.Sites.JobChIN.Api.Attributes;
using System.Net.Http;
using System.Web.Http;

namespace Mlok.Web.Sites.JobChIN.Api
{
    public class JobChINUserApiController : WebCentrumAngularApiModuleController<JobChINModule>
    {
        /// <summary>
        /// User can suggest the hard skill.
        /// </summary>
        /// <param name="name">Name of the suggested hard skill.</param>
        [HttpPost]
        [JobChINAuthorize]
        public HttpResponseMessage SuggestHardSkill(string name)
        {
            return Module.SuggestHardSkill(name).ToOk(Request);
        }
    }
}