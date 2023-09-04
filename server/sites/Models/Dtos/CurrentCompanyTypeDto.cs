using System;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class CurrentCompanyTypeDto
    {
        public string Name { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public int CurrentNumberOfWorkPosition { get; set; }
        public int? NumberOfWorkPosition { get; set; }
        public int CurrentNumberOfStudentsRevealed { get; set; }
        public int? NumberOfStudentsRevealed { get; set; }
        public bool CompanyPresentation { get; set; }
        public bool DatabaseSearch { get; set; }
    }
}