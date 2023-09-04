using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Web.Sites.JobChIN.Models.StudentModels;
using Mlok.Web.Sites.JobChIN.Utils;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    [Validator(typeof(SkillsDtoValidator))]
    public class SkillsDto
    {
        public IEnumerable<int> HardSkills { get; set; }
        public FileUploadDto EducationCertificate { get; set; }
        public IEnumerable<StudentSoftSkill> SoftSkills { get; set; }
        public IEnumerable<LanguageModel> Languages { get; set; }
        public FileUploadDto LanguageCertificate { get; set; }

        public class SkillsDtoValidator : AbstractValidator<SkillsDto>
        {
            public SkillsDtoValidator()
            {
                RuleFor(x => x.Languages)
                    .ListUniqueness(this.Localize("Jazyky", ""), x => x.LanguageId); // TODO: translate

                RuleFor(x => x.SoftSkills)
                    .ListUniqueness(this.Localize("Měkké dovednosti", ""), x => x.SoftSkillId); // TODO: translate

                RuleFor(x => x.HardSkills)
                    .ListUniqueness(this.Localize("Tvrdé dovednosti", "")); // TODO: translate
            }
        }
    }
}