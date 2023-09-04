using System.Web.Http.Controllers;

namespace Mlok.Web.Sites.JobChIN.Api.Attributes
{
    public class StudentAuthorizeAttribute : JobChINAuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return Module.StudentService.GetCurrent() != null;
        }
    }
}