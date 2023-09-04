using Mlok.Modules.WebData;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class AngularConfigDto
    {
        public string StudentAfterLogin { get; set; }
        public IEnumerable<AreaOfInterestDto> AreaOfInterests { get; set; }
        public IEnumerable<HardSkillDto> HardSkills { get; set; }
        public IEnumerable<SoftSkillDto> SoftSkills { get; set; }
        public IEnumerable<WorkerCountRangeDto> WorkerCountRanges { get; set; }
        public IEnumerable<EnumerablePickerValue<int, string>> Countries { get; set; }
        public IEnumerable<EnumerablePickerValue<string, string>> LocalAdministrativeUnits { get; set; }
        public IEnumerable<EnumerablePickerValue<string, string>> Faculties { get; set; }
        public IEnumerable<LanguageDto> Languages { get; set; }
        public IEnumerable<ContractTypeDto> ContractTypes { get; set; }
    }
}