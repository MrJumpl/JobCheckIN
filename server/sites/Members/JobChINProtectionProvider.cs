using Mlok.Modules.EntityProtection;
using Mlok.Umbraco;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.PublishedCache;

namespace Mlok.Web.Sites.JobChIN.Members
{
    [ProtectionProvider(ConfigDocTypeAlias = "JobChIN_ProtectionProvider")]
    public class JobChINProtectionProvider : UmbracoMembersProtectionProvider
    {
        public JobChINProtectionProvider(EntityProtectionModule module) : base(module)
        {
        }

        public JobChINProtectionProvider(MemberPublishedContent currentMember, EntityProtectionModule module) : base(currentMember, module)
        {
        }

        public override string Identifier => "JobChIN";

        public override bool HasUserAccess(IPublishedContent config)
        {
            var siteHelper = SiteHelperResolver.Current.Get<JobChINSiteHelper>(config.Id);
            var module = siteHelper.JobChINModule;
            var protectionConfig = new ProtectionConfig(config);

            if (protectionConfig.CheckStudent() && module.StudentService.GetCurrent() != null)
                return true;

            if (protectionConfig.CheckCompany() && module.CompanyService.GetCurrent() != null)
                return true;

            return false;
        }

        public override ActionResult LoginChallenge(string returnUrl)
        {
            var siteHelper = SiteHelperResolver.Current.Get<JobChINSiteHelper>();
            var member = siteHelper.JobChINModule.MembersPlugin.GetCurrentMember();

            if (member != null)
            {
                var accessDeniedNode = Module.AccessDeniedNode;
                return new RedirectResult(accessDeniedNode.Url);
            }
            else
            {
                return new RedirectResult(Module.Site.MembersModule().MembersPlugin.AngularAppNode.Url + "/login?returnUrl=" + HttpUtility.UrlEncode(returnUrl));
            }
        }
    }
}