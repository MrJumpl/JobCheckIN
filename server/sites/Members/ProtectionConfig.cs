using Mlok.Web.Sites.JobChIN.Models;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Mlok.Web.Sites.JobChIN.Members
{
    public class ProtectionConfig
    {
        private readonly IPublishedContent configNode;

        public ProtectionConfig(IPublishedContent configNode)
        {
            this.configNode = configNode;
        }

        public bool CheckStudent() => configNode.GetPropertyValue<UserType>("users").HasFlag(UserType.Student);

        public bool CheckCompany() => configNode.GetPropertyValue<UserType>("users").HasFlag(UserType.Company);
    }
}