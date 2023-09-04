namespace Mlok.Web.Sites.JobChIN.Models.StudentModels
{
    public class Visibility : IAnonymizable
    {
        public bool ProvideContact { get; set; }

        public void AnonymizeData()
        {
            ProvideContact = false;
        }
    }
}