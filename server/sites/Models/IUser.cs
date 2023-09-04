using Umbraco.Web.PublishedCache;

namespace Mlok.Web.Sites.JobChIN.Models
{
    public interface IUser
    {
        MemberPublishedContent Member { get; set; }
        string EmailForNotiofications();
    }
}