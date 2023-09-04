using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Utils;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.WorkPositionModels
{
    [Validator(typeof(CandidatesValidator))]
    [ModelEditor(typeof(CandidatesWebDataFormatter))]
    public class Candidates
    {
        public bool ActiveDriver { get; set; }
        public bool DrivingLicense { get; set; }

        public IEnumerable<int> AreaOfInterests { get; set; }
        public IEnumerable<int> HardSkills { get; set; }
        public IEnumerable<int> SoftSkills { get; set; }
        public IEnumerable<WorkPositionLanguageModel> Languages { get; set; }
        public IEnumerable<string> Faculties { get; set; }


        public class CandidatesWebDataFormatter : AbstractModelEditor<Candidates, JobChINModule>
        {
            public CandidatesWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Oblasti zájmu", x => x.AreaOfInterests)
                        .SetDataType(x => x.MultipleValuePicker(() => Module.AreaOfInterestController.GetPicker()));
                    cfg.AddField("Tvrdé dovednosti", x => x.HardSkills)
                        .SetDataType(x => x.MultipleValuePicker(() => Module.HardSkillController.GetPicker()));
                    cfg.AddField("Měkké dovednosti", x => x.SoftSkills)
                        .SetDataType(x => x.MultipleValuePicker(() => Module.SoftSkillController.GetPicker()));
                    cfg.AddField("Jazyky", x => x.Languages);
                    cfg.AddField("Fakulta", x => x.Faculties)
                        .SetDataType(x => x.MultipleValuePicker(() => Module.FacultyController.GetPicker()));
                    cfg.AddField("Řidičský průkaz", x => x.DrivingLicense)
                        .SetDataType(x => x.Boolean());
                    cfg.AddField("Aktivní řidič", x => x.ActiveDriver)
                        .SetDataType(x => x.Boolean());

                });
            }
        }

        public class CandidatesValidator : AbstractValidator<Candidates>
        {
            public CandidatesValidator()
            {
                RuleFor(x => x.AreaOfInterests)
                    .ListUniqueness(this.Localize("Oblasti zájmu", "")); // TODO: translate

                RuleFor(x => x.HardSkills)
                    .ListUniqueness(this.Localize("Tvrdé dovednosti", "")); // TODO: translate

                RuleFor(x => x.SoftSkills)
                    .ListUniqueness(this.Localize("Měkké dovednosti", "")); // TODO: translate

                RuleFor(x => x.Languages)
                    .ListUniqueness(this.Localize("Jazyky", "")); // TODO: translate

                RuleFor(x => x.Faculties)
                    .ListUniqueness(this.Localize("Fakulta", "")); // TODO: translate
            }
        }
    }
}