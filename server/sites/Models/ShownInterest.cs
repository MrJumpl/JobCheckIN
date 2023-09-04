using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Web.Sites.JobChIN.Constants;

namespace Mlok.Web.Sites.JobChIN.Models
{
    [Validator(typeof(ShownInterestValidator))]
    public class ShowInterest
    {
        public int WorkPositionId { get; set; }
        public string AdditionalQuestion { get; set; }
        public string CoverLetter { get; set; }
        

        public class ShownInterestValidator : AbstractValidator<ShowInterest>
        {
            public ShownInterestValidator()
            {
                RuleFor(x => x.AdditionalQuestion)
                    .MaximumLength(WebDataConstants.MaximumShortRteLength)
                    .WithName(_ => this.Localize("Doplňující otázka", "Additional question"));

                RuleFor(x => x.CoverLetter)
                    .MaximumLength(WebDataConstants.MaximumShortRteLength)
                    .WithName(_ => this.Localize("Motivační dopis", "Cover letter"));
            }
        }
    }
}