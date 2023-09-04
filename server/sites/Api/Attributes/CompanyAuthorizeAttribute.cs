using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using System.Web.Http.Controllers;

namespace Mlok.Web.Sites.JobChIN.Api.Attributes
{
    public class CompanyAuthorizeAttribute : JobChINAuthorizeAttribute
    {
        public Role? Permissions { get; set; }
        
        /// <summary>
        /// User has to be sign in as company user.
        /// </summary>
        public CompanyAuthorizeAttribute() { }

        /// <summary>
        /// User has to have any of given role.
        /// </summary>
        public CompanyAuthorizeAttribute(Role roles)
        {
            Permissions = roles;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var user = Module.CompanyService.GetCurrent();
            if (user == null)
                return false;
            if (!Permissions.HasValue)
                return true;

            return Permissions.Value.HasFlag(user.Role);
        }
    }
}