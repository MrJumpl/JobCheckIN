using Mlok.Modules.WebData.Members;

namespace Mlok.Web.Sites.JobChIN
{
    public interface IJobChINModule : IWebDataMembersModule
    {
        ISettings Settings { get; }
    }
}