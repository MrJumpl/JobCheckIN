using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;

namespace Mlok.Web.Sites.JobChIN.Models
{
    [Validator(typeof(AreaOfInterestValidator))]
    [ModelEditor(typeof(AreaOfInterestWebDataFormatter))]
    public class AreaOfInterest
    {
        public int AreaOfInterestId { get; set; }
        public WebDataI18N<string> Name { get; set; }

        public class AreaOfInterestWebDataFormatter : AbstractModelEditor<AreaOfInterest, JobChINModule>
        {
            public AreaOfInterestWebDataFormatter()
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
                    });
                });
            }
        }

        public class AreaOfInterestValidator : AbstractValidator<AreaOfInterest>
        {
            public AreaOfInterestValidator()
            {
                RuleFor(x => x.Name)
                    .RequireLanguages(WebDataConstants.RequiredLanguages);
            }
        }
    }
}