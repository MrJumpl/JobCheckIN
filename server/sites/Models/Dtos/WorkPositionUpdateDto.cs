using Mlok.Web.Sites.JobChIN.Models.WorkPositionModels;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class WorkPositionUpdateDto
    {
        public int WorkPositionId { get; set; }
        public BasicInfo BasicInfo { get; set; }
        public Detail Detail { get; set; }
        public Candidates Candidates { get; set; }
        public CandidateRequest CandidateRequest { get; set; }
    }
}