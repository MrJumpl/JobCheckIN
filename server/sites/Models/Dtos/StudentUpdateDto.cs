using Mlok.Web.Sites.JobChIN.Models.StudentModels;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class StudentUpdateDto
    {
        public BasicInfo BasicInfo { get; set; }
        public StudentPhotoDto Photo { get; set; }
        public Studies Studies { get; set; }
        public WorkExperiences WorkExperiences { get; set; }
        public SkillsDto Skills { get; set; }
        public Contact Contact { get; set; }
        public NotificationSettings NotificationSettings { get; set; }
        public Visibility Visibility { get; set; }
    }
}