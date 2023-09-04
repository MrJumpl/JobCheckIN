using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mlok.Web.Sites.JobChIN.Models.CompanyModels
{
    [Validator(typeof(GeneralInfoValidator))]
    [ModelEditor(typeof(GeneralInfoWebDataFormatter))]
    public class GeneralInfo : IAnonymizable
    {
        public string Ico { get; set; }
        public string Dic { get; set; }
        public string CompanyName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
        public string CorrespondenceStreet { get; set; }
        public string CorrespondenceCity { get; set; }
        public string CorrespondenceZipCode { get; set; }
        public int? CorrespondenceCountryId { get; set; }
        public int WorkerCountRangeId { get; set; }
        public Sector Sector { get; set; }
        public bool GdprAgreement { get; set; }

        public void AnonymizeData()
        {
            Ico = "00000000";
            Dic = null;
            CompanyName = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            ZipCode = string.Empty;
            CountryId = 0;
            CorrespondenceStreet = null;
            CorrespondenceCity = null;
            CorrespondenceZipCode = null;
            CorrespondenceCountryId = null;
            WorkerCountRangeId = 0;
        }


        public class GeneralInfoWebDataFormatter : AbstractModelEditor<GeneralInfo, JobChINModule>
        {
            public GeneralInfoWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("IČO", x => x.Ico);
                    cfg.AddField("DIČ", x => x.Dic);
                    cfg.AddField("Název společnosti", x => x.CompanyName);
                    cfg.AddField("Ulice", x => x.Street);
                    cfg.AddField("Město", x => x.City);
                    cfg.AddField("PSČ", x => x.ZipCode);
                    cfg.AddField("Stát", x => x.CountryId)
                        .SetDataType(x => x.SingleValuePicker(() => Module.CountryController.GetPicker()));
                    cfg.AddField("Zařaďte Vaši společnost do sektoru", x => x.Sector)
                        .SetDataType(x => x.Toggle<Sector>());
                    cfg.AddField("Jaký máte počet zaměstnanců v ČR?", x => x.WorkerCountRangeId)
                        .SetDataType(x => x.SingleValuePicker(() => Module.WorkerCountRangeController.GetPicker()));
                    cfg.AddHelpText("Korespondenční adresa");
                    cfg.AddField("Ulice", x => x.CorrespondenceStreet);
                    cfg.AddField("Město", x => x.CorrespondenceCity);
                    cfg.AddField("PSČ", x => x.CorrespondenceZipCode);
                    cfg.AddField("Stát", x => x.CorrespondenceCountryId)
                        .SetDataType(x => x.SingleValuePicker(() => Module.CountryController.GetCountries().Select(y => EnumerablePickerValue.From<int?, string>(y.Kod, y.Nazev_cs))));
                    cfg.AddField("Gdpr souhlas", x => x.GdprAgreement)
                        .SetDataType(x => x.Boolean());
                });
            }
        }

        public class GeneralInfoValidator : AbstractValidator<GeneralInfo>
        {
            public GeneralInfoValidator()
            {
                RuleFor(x => x.GdprAgreement)
                    .Equal(true);

                RuleFor(x => x.Ico)
                    .Length(WebDataConstants.IcoLength)
                    .WithName(_ => this.Localize("IČO", "IČO"));
                
                RuleFor(x => x.Ico)
                    .Must(x => Regex.Match(x, @"\d+").Success)
                    .WithMessage(_ => this.Localize("Pole 'IČO' neobsahuje pouze čísla", "")); // TODO: translate

                RuleFor(x => x.Dic)
                    .MaximumLength(WebDataConstants.MaximumDicLength)
                    .WithName(_ => this.Localize("DIČ", "DIČ"));

                RuleFor(x => x.CompanyName)
                    .MaximumLength(WebDataConstants.MaximumNameLength)
                    .NotEmpty()
                    .WithName(_ => this.Localize("Název společnosti", "Company name"));

                RuleFor(x => x.Street)
                    .MaximumLength(WebDataConstants.MaximumStreetLength)
                    .NotEmpty()
                    .WithName(_ => this.Localize("Ulice a č. p.", "Street and no."));

                RuleFor(x => x.City)
                    .MaximumLength(WebDataConstants.MaximumCityLength)
                    .NotEmpty()
                    .WithName(_ => this.Localize("Město", "City"));

                RuleFor(x => x.ZipCode)
                    .MaximumLength(WebDataConstants.MaximumZipCodeLength)
                    .NotEmpty()
                    .WithName(_ => this.Localize("PSČ", "Zip code"));

                RuleFor(x => x.CountryId)
                    .GreaterThan(0)
                    .WithMessage(_ => this.Localize("Pole 'Stát' není správně vyplněno", "")); // TODO: translate

                RuleFor(x => x.WorkerCountRangeId)
                    .GreaterThan(0)
                    .WithMessage(_ => this.Localize("Pole 'Jaký máte počet zaměstnanců v ČR?' není správně vyplněno", "")); // TODO: translate

                RuleFor(x => x.Sector)
                    .NotEmpty()
                    .Must(x => x == Models.Sector.PrivateSector || x == Models.Sector.NonProfitSector || x == Models.Sector.PublicSector)
                    .WithMessage(_ => this.Localize("Pole 'Zařaďte Vaši společnost do sektoru' není správně vyplněno", "")); // TODO: translate

                RuleFor(x => x.CorrespondenceStreet)
                    .MaximumLength(WebDataConstants.MaximumStreetLength)
                    .WithName(_ => this.Localize("Korespondenční adresa - Ulice a č. p.", "Korespondenční adresa - Street and no."));

                RuleFor(x => x.CorrespondenceStreet)
                    .NotEmpty()
                    .When(x => !string.IsNullOrWhiteSpace(x.CorrespondenceCity) || !string.IsNullOrWhiteSpace(x.CorrespondenceZipCode) || x.CorrespondenceCountryId.HasValue)
                    .WithMessage(_ => this.Localize("Korespondenční adresa není správně vyplněna", "")); // TODO: translate

                RuleFor(x => x.CorrespondenceCity)
                    .MaximumLength(WebDataConstants.MaximumCityLength)
                    .WithName(_ => this.Localize("Korespondenční adresa - Město", "Korespondenční adresa - City"));

                RuleFor(x => x.CorrespondenceCity)
                    .NotEmpty()
                    .When(x => !string.IsNullOrWhiteSpace(x.CorrespondenceStreet) || !string.IsNullOrWhiteSpace(x.CorrespondenceZipCode) || x.CorrespondenceCountryId.HasValue)
                    .WithMessage(_ => this.Localize("Korespondenční adresa není správně vyplněna", "")); // TODO: translate

                RuleFor(x => x.CorrespondenceZipCode)
                    .MaximumLength(WebDataConstants.MaximumZipCodeLength)
                    .WithName(_ => this.Localize("Korespondenční adresa - PSČ", "Korespondenční adresa - Zip code"));

                RuleFor(x => x.CorrespondenceZipCode)
                    .NotEmpty()
                    .When(x => !string.IsNullOrWhiteSpace(x.CorrespondenceStreet) || !string.IsNullOrWhiteSpace(x.CorrespondenceCity) || x.CorrespondenceCountryId.HasValue)
                    .WithMessage(_ => this.Localize("Korespondenční adresa není správně vyplněna", "")); // TODO: translate

                RuleFor(x => x.CorrespondenceCountryId)
                    .GreaterThan(0)
                    .WithMessage(_ => this.Localize("Pole 'Korespondenční adresa - Stát' není správně vyplněno", "")); // TODO: translate

                RuleFor(x => x.CorrespondenceCountryId)
                    .NotEmpty()
                    .When(x => !string.IsNullOrWhiteSpace(x.CorrespondenceStreet) || !string.IsNullOrWhiteSpace(x.CorrespondenceCity) || !string.IsNullOrWhiteSpace(x.CorrespondenceZipCode))
                    .WithMessage(_ => this.Localize("Korespondenční adresa není správně vyplněna", "")); // TODO: translate

            }
        }
    }
}