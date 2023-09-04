using Mlok.Web.Sites.JobChIN.Models.StudentModels;
using System;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class StudentAngularConfigDto
    {
        public int StudentId { get; set; }
        public int Uco { get; set; }
        public string FullName { get; set; }
        public IEnumerable<Study> MuniStudies { get; set; }
        public DateTime? LastTimeUpdatedByStudent { get; set; }
        public FormDescriptionsDto FormDescriptions { get; set; }
        public StudentUpdateDto Model { get; set; }
    }
}