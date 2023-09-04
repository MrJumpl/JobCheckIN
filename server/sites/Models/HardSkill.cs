using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;

namespace Mlok.Web.Sites.JobChIN.Models
{
    [Validator(typeof(HardSkillValidator))]
    [ModelEditor(typeof(HardSkillWebDataFormatter))]
    public class HardSkill
    {
        public int HardSkillId { get; set; }
        public WebDataI18N<string> Name { get; set; }
        public int AreaOfInterestId { get; set; }
        public string MuPartId { get; set; }

        public class HardSkillWebDataFormatter : AbstractModelEditor<HardSkill, JobChINModule>
        {
            public HardSkillWebDataFormatter()
            {
                Setup(cfg =>
                {
                    cfg.SetName("Název", x => x.Name, namePropertyHide: true);
                });

                SetupOverview(listviewCfg => { });

                SetupModelEditor(detailCfg =>
                {
                    detailCfg.AddTab("Metadata", SetupSimpleModelEditor);
                });

                SetupSimpleModelEditor(SetupSimpleModelEditor);
            }

            void SetupSimpleModelEditor (SimpleModelEditorConfig<HardSkill> cfg)
            {
                cfg.AddField("Název", x => x.Name);
                cfg.AddField("Oblast zájmu", x => x.AreaOfInterestId)
                    .SetDataType(x => x.SingleValuePicker(() => Module.AreaOfInterestController.GetPicker(), true));
                cfg.AddField("Falkuta/Katedra", x => x.MuPartId)
                    .SetDataType(x => x.DepartmentPicker());
            }
        }
        
        public class HardSkillValidator : AbstractValidator<HardSkill>
        {
            public HardSkillValidator()
            {
                RuleFor(x => x.Name)
                    .RequireLanguages(WebDataConstants.RequiredLanguages)
                    .WithName(x => this.Localize("Název", "Name"));

                RuleFor(x => x.AreaOfInterestId)
                    .NotEmpty()
                    .WithName(x => this.Localize("Oblast zájmu", "Area of interest"));
            }
        }
    }
}