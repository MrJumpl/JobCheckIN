using Mlok.Core.Models;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;

namespace Mlok.Web.Sites.JobChIN.Models
{
    public class InviteCompanyUserModel : JsonStorageModel
    {
        public string Email { get; set; }
        public Role Role { get; set; }
        public int CompanyId { get; set; }
        public int? MemberId { get; set; }

    }
}