using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Web.Sites.JobChIN.Constants;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    [Validator(typeof(PresentationDtoValidator))]
    public class PresentationDto
    {
        public FileUploadDto Logo { get; set; }
        public FileUploadDto BackgroundImage { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Differences { get; set; }
        public string InterviewDescription { get; set; }
        public string Web { get; set; }
        public string Linkedin { get; set; }
        public string Facebook { get; set; }


        public class PresentationDtoValidator : AbstractValidator<PresentationDto>
        {
            public PresentationDtoValidator()
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