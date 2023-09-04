using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;

namespace Mlok.Web.Sites.JobChIN.Models
{
    [Validator(typeof(LanguageValidator))]
    [ModelEditor(typeof(LanguageWebDataFormatter))]

    public class Language
    {
        public int LanguageId { get; set; }
        public WebDataI18N<string> Name { get; set; }
        public string Shorcut { get; set; }

        public class LanguageWebDataFormatter : AbstractModelEditor<Language, JobChINModule>
        {
            public LanguageWebDataFormatter()
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
                        tabConfig.AddField("Zkratka", x => x.Shorcut);
                    });
                });
            }
        }

        public class LanguageValidator : AbstractValidator<Language>
        {
            public LanguageValidator()
            {
                RuleFor(x => x.Name)
                    .RequireLanguages(WebDataConstants.RequiredLanguages)
                    .MaximumLength(10);

                RuleFor(x => x.Shorcut)
                    .MaximumLength(5);
            }
        }
    }
}