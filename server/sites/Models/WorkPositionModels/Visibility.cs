namespace Mlok.Web.Sites.JobChIN.Models.WorkPositionModels
{
    public class Visibility : IAnonymizable
    {
        public bool Hidden { get; set; }

        public void AnonymizeData()
        {
            Hidden = true;
        }
    }
}