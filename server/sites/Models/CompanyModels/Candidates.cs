using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;
using Mlok.Web.Sites.JobChIN.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Models.CompanyModels
{
    [Validator(typeof(CandidatesValidator))]
    [ModelEditor(typeof(CandidatesWebDataFormatter))]
    public class Candidates : IAnonymizable
    {
        public IEnumerable<int> AreaOfInterests { get; set; }
        public IEnumerable<string> Faculties { get; set; }
        public IEnumerable<LanguageModel> LanguageSkillPrefered { get; set; }
        public string PeopleTypesSought { get; set; }

        public void AnonymizeData()
        {
            PeopleTypesSought = null;
        }


        public class CandidatesWebDataFormatter : AbstractModelEditor<Candidates, JobChINModule>
        {
            public CandidatesWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Preferované oblasti zájmu", x => x.AreaOfInterests)
                        .SetDataType(x => x.MultipleValuePicker(() =>
                            Module.AreaOfInterestController.GetPicker()));
                    cfg.AddField("Preferované fakulty", x => x.Faculties)
                        .SetDataType(x => x.MultipleValuePicker(() =>
                            Module.FacultyController.GetPicker()));
                    cfg.AddField("Preferované jazyky", x => x.LanguageSkillPrefered)
                        .SetDescription("Nemohou být vybrány stejné jazyky");
                    cfg.AddField("Co požadujete, aby uměli vaši uchazeči? Jací by měli být?", x => x.PeopleTypesSought)
                        .SetDataType(x => x.Rte());
                });
            }
        }

        public class CandidatesValidator : AbstractValidator<Candidates>
        {
            public CandidatesValidator()
            {
                RuleFor(x => x.AreaOfInterests)
                    .ListUniqueness(this.Localize("Preferované oblasti zájmu", "")); // TODO: translate

                RuleFor(x => x.Faculties)
                    .ListUniqueness(this.Localize("Preferované fakulty", "")); // TODO: translate

                RuleFor(x => x.LanguageSkillPrefered)
                    .ListUniqueness(this.Localize("Preferované jazyky", "")); // TODO: translate

                RuleFor(x => x.PeopleTypesSought)
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Co požadujete, aby uměli vaši uchazeči? Jací by měli být?", "")); // TODO: translate

                RuleFor(x => x.LanguageSkillPrefered)
                    .Must(x => x.Select(y => y.LanguageId).Distinct().Count() == x.Count())
                    .WithMessage(_ => this.Localize("Pole 'preferované jazyky' nesmí obsahovat dva stejné jazyky.", "")); // TODO: translate
            }
        }
    }
}