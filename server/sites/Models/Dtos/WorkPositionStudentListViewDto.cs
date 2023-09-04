using System;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class WorkPositionStudentListViewDto
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public DateTime? ShownInterest { get; set; }
        public string PhotoLink { get; set; }
        public MatchCategory Match { get; set; }
    }
}