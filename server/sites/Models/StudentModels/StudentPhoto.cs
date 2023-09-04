namespace Mlok.Web.Sites.JobChIN.Models.StudentModels
{
    public class StudentPhoto : IAnonymizable
    {
        public int? Photo { get; set; }

        public void AnonymizeData()
        {
            Photo = null;
        }
    }
}