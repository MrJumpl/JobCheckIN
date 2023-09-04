using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Models.StudentModels
{
    [Validator(typeof(StudentSoftSkillValidator))]
    [ModelEditor(typeof(StudentSoftSkillWebDataFormatter))]
    public class StudentSoftSkill
    {
        public int SoftSkillId { get; set; }
        public bool IsPrimary { get; set; }
        public string Description { get; set; }


        public class StudentSoftSkillWebDataFormatter : AbstractModelEditor<StudentSoftSkill, JobChINModule>
        {
            public StudentSoftSkillWebDataFormatter()
            {
                Setup(cfg => 
                {
                    cfg.SetName("Název", x => x.GetTitle(Module));
                });

                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Dovednost", x => x.SoftSkillId)
                        .SerializeAs(x => x == default(int) ? null : (int?)x, x => x ?? default(int))
                        .SetDataType(x => x.SingleValuePicker(() => Module.SoftSkillController.GetPicker()));
                    cfg.AddField("Primární", x => x.IsPrimary)
                        .SetDataType(x => x.Boolean());
                    cfg.AddField("Vysvětlete, proč si to o sobě myslíte", x => x.Description)
                        .SetDataType(x => x.Rte());
                });
            }
        }

        public class StudentSoftSkillValidator : AbstractValidator<StudentSoftSkill>
        {
            public StudentSoftSkillValidator()
            {
                RuleFor(x => x.SoftSkillId)
                    .GreaterThan(default(int))
                    .WithMessage(_ => this.Localize("Pole 'Dovednost' musí být vyplněno.", "")); // TODO: translate
                RuleFor(x => x.Description)
                    .NotEmpty()
                    .MaximumLength(WebDataConstants.MaximumShortRteLength)
                    .WithName(_ => this.Localize("Vysvětlete, proč si to o sobě myslíte", "")); // TODO: translate
            }
        }

        string GetTitle(JobChINModule module)
        {
            return module.SoftSkillController.GetPicker().FirstOrDefault(x => x.Value == SoftSkillId)?.DisplayValue;
        }

    }
}