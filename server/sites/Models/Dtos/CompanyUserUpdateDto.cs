using Mlok.Web.Sites.JobChIN.Models.CompanyModels;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class CompanyUserUpdateDto
    {
        public ContactPerson ContactPerson { get; set; }
        public NotificationSettings NotificationSettings { get; set; }
    }
}