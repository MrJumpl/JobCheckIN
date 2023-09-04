using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;

namespace Mlok.Web.Sites.JobChIN.Models.CompanyModels
{
    [Validator(typeof(BranchValidator))]
    [ModelEditor(typeof(BranchWebDataFormatter))]
    public class Branch
    {
        public int BranchId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string LocationId { get; set; }


        public class BranchWebDataFormatter : AbstractModelEditor<Branch, JobChINModule>
        {
            public BranchWebDataFormatter()
            {
                Setup(cfg =>
                {
                    cfg.SetName("Název", x => x.GetTitle());
                });

                SetupOverview(listviewCfg => { });

                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Město", x => x.City);
                    cfg.AddField("Ulice", x => x.Street);
                    cfg.AddField("PSČ", x => x.ZipCode);
                    cfg.AddField("Umístění pobočky", x => x.LocationId)
                        .SetDataType(x => x.SingleValuePicker(() => Module.LocalAdministrativeUnitsController.GetPicker(), enableSearch: true));
                });
            }
        }

        public class BranchValidator : AbstractValidator<Branch>
        {
            public BranchValidator()
            {
                RuleFor(x => x.City)
                    .NotEmpty()
                    .MaximumLength(WebDataConstants.MaximumCityLength)
                    .WithName(x => this.Localize("Město", "City"));

                RuleFor(x => x.Street)
                    .NotEmpty()
                    .MaximumLength(WebDataConstants.MaximumStreetLength)
                    .WithName(x => this.Localize("Ulice", "Street"));

                RuleFor(x => x.ZipCode)
                    .NotEmpty()
                    .MaximumLength(WebDataConstants.MaximumZipCodeLength)
                    .WithName(x => this.Localize("PSČ", "ZipCode"));

                RuleFor(x => x.LocationId)
                    .NotEmpty()
                    .WithName(x => this.Localize("Umístění pobočky", "")); // TODO: translate
            }
        }
 
        public string GetTitle()
        {
            string result = Street ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(Street) && !string.IsNullOrWhiteSpace(City))
                result += " - ";
            result += City ?? string.Empty;
            return result;
        }
    }
}