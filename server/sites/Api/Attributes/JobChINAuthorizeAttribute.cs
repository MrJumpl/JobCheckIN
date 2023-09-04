using Mlok.Modules.WebData.Members;

namespace Mlok.Web.Sites.JobChIN.Api.Attributes
{
    public class JobChINAuthorizeAttribute : MembersApiAuthorizeAttribute
    {
        protected JobChINModule Module => Site.ResolveModule<JobChINModule>(ModuleNode);
    }
}