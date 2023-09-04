using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;

namespace Mlok.Web.Sites.JobChIN.Models.CompanyModels
{
    [Validator(typeof(PresentationValidator))]
    [ModelEditor(typeof(PresentationWebDataFormatter))]
    public class Presentation : IAnonymizable
    {
        public int? Logo { get; set; }
        public int? BackgroundImage { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Differences { get; set; }
        public string InterviewDescription { get; set; }
        public string Web { get; set; }
        public string Linkedin { get; set; }
        public string Facebook { get; set; }

        public void AnonymizeData()
        {
            Logo = null;
            BackgroundImage = null;
            ShortDescription = null;
            Description = null;
            Differences = null;
            InterviewDescription = null;
            Web = null;
            Linkedin = null;
            Facebook = null;
        }


        public class PresentationWebDataFormatter : AbstractModelEditor<Presentation, JobChINModule>
        {
            public PresentationWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Jak byste krátce představili Vaši společnost?", x => x.ShortDescription)
                        .SetDataType(x => x.MultilineText());
                    cfg.AddField("Jak byste poutavě představili Vaši společnost?", x => x.Description)
                        .SetDataType(x => x.Rte());
                    cfg.AddField("Popište svoji firemní kulturu. Čím je specifická?", x => x.Differences)
                        .SetDataType(x => x.Rte());
                    cfg.AddField("Popis pohovoru", x => x.InterviewDescription)
                        .SetDataType(x => x.Rte());
                    cfg.AddField("Webové stránky", x => x.Web);
                    cfg.AddField("Firemní LinkedIn", x => x.Linkedin)
                        .SetDescription("Část adresy nacházející se za 'www.linkedin.com/in/'");
                    cfg.AddField("Firemní Facebook", x => x.Facebook)
                        .SetDescription("Část adresy nacházející se za 'www.facebook.com/'");

                });
            }
        }

        public class PresentationValidator : AbstractValidator<Presentation>
        {
            public PresentationValidator()
            {
                RuleFor(x => x.ShortDescription)
                    .MaximumLength(WebDataConstants.MaximumDescLength)
                    .WithName(_ => this.Localize("Jak byste krátce představili Vaši společnost?", "")); // TODO: translate

                RuleFor(x => x.Description)
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Jak byste poutavě představili Vaši společnost?", "")); // TODO: translate

                RuleFor(x => x.Differences)
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Popište svoji firemní kulturu. Čím je specifická?", "")); // TODO: translate

                RuleFor(x => x.Differences)
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Jak byste poutavě představili Vaši společnost?", "")); // TODO: translate

                RuleFor(x => x.Web)
                    .MaximumLength(WebDataConstants.MaximumSocialMediaLenght)
                    .WithName(_ => this.Localize("Webové stránky", "")); // TODO: translate

                RuleFor(x => x.Linkedin)
                    .MaximumLength(WebDataConstants.MaximumSocialMediaLenght)
                    .WithName(_ => this.Localize("Firemní LinkedIn", "")); // TODO: translate

                RuleFor(x => x.Facebook)
                    .MaximumLength(WebDataConstants.MaximumSocialMediaLenght)
                    .WithName(_ => this.Localize("Firemní Facebook", "")); // TODO: translate
            }
        }
    }
}