using Mlok.Web.Sites.JobChIN.Models.WorkPositionModels;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class SearchStudentFilterDto
    {
        public int PageNo { get; set; }
        public IEnumerable<int> ContractTypes { get; set; }
        public bool ActiveDriver { get; set; }
        public bool DrivingLicense { get; set; }
        public IEnumerable<int> AreaOfInterests { get; set; }
        public IEnumerable<int> HardSkills { get; set; }
        public IEnumerable<int> SoftSkills { get; set; }
        public IEnumerable<WorkPositionLanguageModel> Languages { get; set; }
        public IEnumerable<string> Faculties { get; set; }
    }
}