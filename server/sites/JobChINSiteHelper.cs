using Mlok.Umbraco;

namespace Mlok.Web.Sites.JobChIN
{
    [SiteHelper(657768)]
    public class JobChINSiteHelper : SiteHelper
    {
        public JobChINSiteHelper() : base()
        { }

        public JobChINSiteHelper(int contentId) : base(contentId)
        { }

        private JobChINModule _jobChINModule;
        public JobChINModule JobChINModule
            => _jobChINModule ?? (_jobChINModule = ResolveModule<JobChINModule>("JobCheckINModule"));
    }
}