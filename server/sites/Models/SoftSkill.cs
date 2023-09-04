using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;

namespace Mlok.Web.Sites.JobChIN.Models
{
    [Validator(typeof(SoftSkillValidator))]
    [ModelEditor(typeof(SoftSkillWebDataFormatter))]
    public class SoftSkill
    {
        public int SoftSkillId { get; set; }
        public WebDataI18N<string> Name { get; set; }
        public WebDataI18N<string> Description { get; set; }


        public class SoftSkillWebDataFormatter : AbstractModelEditor<SoftSkill, JobChINModule>
        {
            public SoftSkillWebDataFormatter()
            {
                Setup(cfg =>
                {
                    cfg.SetName("Název", x => x.Name, namePropertyHide: true);
                });

                SetupOverview(listviewCfg => { });

                SetupModelEditor(detailCfg =>
                {
                    detailCfg.AddTab("Metadata", tabConfig =>
                    {
                        tabConfig.AddField("Název", x => x.Name);
                        tabConfig.AddField("Popisek", x => x.Description)
                            .SetDataType(x => x.MultilineText());
                    });
                });
            }
        }

        public class SoftSkillValidator : AbstractValidator<SoftSkill>
        {
            public SoftSkillValidator()
            {
                RuleFor(x => x.Name)
                    .RequireLanguages(WebDataConstants.RequiredLanguages);
            }
        }
    }
    }