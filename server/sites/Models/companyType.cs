using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;

namespace Mlok.Web.Sites.JobChIN.Models
{

    [Validator(typeof(CompanyTypeValidator))]
    [ModelEditor(typeof(CompanyTypeWebDataFormatter))]

    public class CompanyType
    {
        public int CompanyTypeId { get; set; }
        public WebDataI18N<string> Name { get; set; }
        public int? PriceProfit { get; set; }
        public int? PriceNonProfit { get; set; }
        public int? NumberOfWorkPosition { get; set; }
        public int? NumberOfStudentsRevealed { get; set; }
        public int Validity { get; set; }
        public bool DatabaseSearch { get; set; }
        public int? PackageProfit { get; set; }
        public int? PackageNonProfit { get; set; }
        public bool CompanyPresentation { get; set; }
        public bool Visible { get; set; }


        public class CompanyTypeWebDataFormatter : AbstractModelEditor<CompanyType, JobChINModule>
        {
            public CompanyTypeWebDataFormatter()
            {
                Setup(cfg =>
                {
                    cfg.SetName("Název", x => x.Name, namePropertyHide: true);
                });

                SetupOverview(listviewCfg => {
                    listviewCfg.AddField("Počet inzerátů", x => x.NumberOfWorkPosition);
                    listviewCfg.AddField("Počet odrkytí", x => x.NumberOfStudentsRevealed);
                    listviewCfg.AddField("Viditelný", x => x.Visible ? "Ano" : "Ne");
                });

                SetupModelEditor(detailCfg =>
                {
                    detailCfg.AddTab("Metadata", tabConfig =>
                    {
                        tabConfig.AddField("Název", x => x.Name);
                        tabConfig.AddHelpText("V případě změny balíčku vytvořte nový typ účtu, který nebude viditelný a bude obsahovat neaktuálnní verzi balíčku a jeho cenu, aby se firmám zobrazovala historie správně");
                        tabConfig.AddField("Cena pro ziskový sektor", x => x.PriceProfit)
                            .SetDataType(x => x.Number());
                        tabConfig.AddField("OC balíček pro ziskový sektor", x => x.PackageProfit)
                            .SetDataType(x => x.Number());
                        tabConfig.AddField("Cena pro neziskový sektor", x => x.PriceNonProfit)
                            .SetDataType(x => x.Number());
                        tabConfig.AddField("OC balíček pro neziskový sektor", x => x.PackageNonProfit)
                            .SetDataType(x => x.Number());
                        tabConfig.AddField("Počet inzerátů", x => x.NumberOfWorkPosition)
                            .SetDescription("Pokud není vyplněno nastaví se na 'neomezeno'")
                            .SetDataType(x => x.Number());
                        tabConfig.AddField("Počet odrkytí studentů", x => x.NumberOfStudentsRevealed)
                            .SetDescription("Pokud není vyplněno nastaví se na 'neomezeno'")
                            .SetDataType(x => x.Number());
                        tabConfig.AddField("Platnost", x => x.Validity)
                            .SetDescription("Počet měsíců, jak dlouho je platný účet")
                            .SetDataType(x => x.Number());
                        tabConfig.AddField("Vyhledávání v databázi studentů", x => x.DatabaseSearch)
                            .SetDataType(x => x.Boolean());
                        tabConfig.AddField("Prezentační profil zaměstnavatele", x => x.CompanyPresentation)
                            .SetDataType(x => x.Boolean());
                        tabConfig.AddField("Viditelný pro zaměstnavatele", x => x.Visible)
                            .SetDescription("Pokud je zašrtnutý zobrazí, zaměstanvatel si ho může vybrat sám")
                            .SetDataType(x => x.Boolean());
                    });
                });
            }
        }

        public class CompanyTypeValidator : AbstractValidator<CompanyType>
        {
            public CompanyTypeValidator()
            {
                RuleFor(x => x.Name)
                    .RequireLanguages(WebDataConstants.RequiredLanguages)
                    .MaximumLength(64);

                RuleFor(x => x.PriceProfit)
                    .GreaterThan(0)
                    .NotEmpty()
                    .When(x => x.PackageProfit.HasValue)
                    .WithName("Cena pro ziskový sektor");

                RuleFor(x => x.PriceNonProfit)
                    .GreaterThan(0)
                    .NotEmpty()
                    .When(x => x.PackageNonProfit.HasValue)
                    .WithName("Cena pro neziskový sektor");

                RuleFor(x => x.PackageProfit)
                    .GreaterThan(0)
                    .NotEmpty()
                    .When(x => !x.PackageNonProfit.HasValue)
                    .WithMessage("Alespoň jeden OC balíček musí být vyplněný");

                RuleFor(x => x.PackageProfit)
                    .GreaterThan(0)
                    .NotEmpty()
                    .When(x => x.Visible)
                    .WithMessage("Pokud je viditelný musí mít vyplněné oba OC balíčky");

                RuleFor(x => x.PackageNonProfit)
                    .GreaterThan(0)
                    .NotEmpty()
                    .When(x => !x.PackageProfit.HasValue)
                    .WithMessage("Alespoň jeden OC balíček musí být vyplněný");

                RuleFor(x => x.PackageNonProfit)
                    .GreaterThan(0)
                    .NotEmpty()
                    .When(x => x.Visible)
                    .WithMessage("Pokud je viditelný musí mít vyplněné oba OC balíčky");
            }
        }

    }
}