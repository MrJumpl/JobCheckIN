using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Models.StudentModels
{
    [Validator(typeof(SkillsValidator))]
    [ModelEditor(typeof(SkillsWebDataFormatter))]
    public class Skills : ICompleteness, IAnonymizable
    {
        public IEnumerable<int> HardSkills { get; set; }
        public int? EducationCertificate { get; set; }
        public IEnumerable<StudentSoftSkill> SoftSkills { get; set; }
        public IEnumerable<LanguageModel> Languages { get; set; }
        public int? LanguageCertificate { get; set; }

        public int Completeness()
        {
            int count = 0;
            if (HardSkills?.Any() ?? false)
                count++;
            if (SoftSkills?.Any() ?? false)
                count++;
            return count;
        }

        public int FullyCompleted() => 2;

        public void AnonymizeData()
        {
            EducationCertificate = null;
            LanguageCertificate = null;
        }


        public class SkillsWebDataFormatter : AbstractModelEditor<Skills, JobChINModule>
        {
            public SkillsWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Tvrdé dovednosti", x => x.HardSkills)
                        .SetDataType(x => x.MultipleValuePicker(() => Module.HardSkillController.GetPicker(), enableSearch: true));
                    cfg.AddField("Měkké dovednosti", x => x.SoftSkills)
                        .SetDescription("Nemohou být přidané stéjné měkké dovednosti");
                    cfg.AddField("Jazyky", x => x.Languages)
                        .SetDescription("Nemohou být vybrány stejné jazyky");
                });
            }
        }

        public class SkillsValidator : AbstractValidator<Skills>
        {
            public SkillsValidator()
            {
                RuleFor(x => x.Languages)
                    .ListUniqueness(this.Localize("Jazyky", ""), x => x.LanguageId); // TODO: translate

                RuleFor(x => x.SoftSkills)
                    .ListUniqueness(this.Localize("Měkké dovednosti", ""), x => x.SoftSkillId); // TODO: translate

                RuleFor(x => x.HardSkills)
                    .ListUniqueness(this.Localize("Tvrdé dovednosti", "")); // TODO: translate

                RuleFor(x => x.SoftSkills)
                    .Must(x => x == null || x.Count(y => y.IsPrimary) <= 3)
                    .WithMessage(_ => this.Localize("Pole 'Hlavní měkké dovednosti' nesmí obsahovat více než 3 hodnosty.", "")); // TODO: translate
            }
        }
    }
}