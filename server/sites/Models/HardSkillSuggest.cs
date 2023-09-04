using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;

namespace Mlok.Web.Sites.JobChIN.Models
{
    [Validator(typeof(HardSkillSuggestValidator))]
    [ModelEditor(typeof(HardSkillSuggestWebDataFormatter))]
    public class HardSkillSuggest
    {
        public int HardSkillSuggestId { get; set; }
        public string Name { get; set; }
        public int MemberId { get; set; }

        public class HardSkillSuggestWebDataFormatter : AbstractModelEditor<HardSkillSuggest, JobChINModule>
        {
            public HardSkillSuggestWebDataFormatter()
            {
                Setup(cfg =>
                {
                    cfg.SetName("Název", x => x.Name);
                });

                SetupOverview(listviewCfg => { });
            }
        }

        public class HardSkillSuggestValidator : AbstractValidator<HardSkillSuggest>
        {
            public HardSkillSuggestValidator()
            {
                RuleFor(x => x.Name)
                    .MaximumLength(WebDataConstants.MaximumNameLength)
                    .WithName(x => this.Localize("Název", "Name"));
            }
        }

    }
}