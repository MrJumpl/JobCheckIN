using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Modules.WebData;
using System;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Models.CompanyModels
{
    [Validator(typeof(CompanyCompanyTypeValidator))]
    [ModelEditor(typeof(CompanyCompanyTypeWebDataFormatter))]
    public class CompanyCompanyType
    {
        public int CompanyCompanyTypeId { get; set; }
        public int CompanyTypeId { get; set; }
        public int? NumberOfWorkPosition { get; set; }
        public int? NumberOfStudentsRevealed { get; set; }
        public bool DatabaseSearch { get; set; }
        public bool CompanyPresentation { get; set; }
        public DateTime ActiveFrom { get; set; } = DateTime.Today;
        public DateTime ActiveTo { get; set; } = DateTime.Today.AddMonths(1).AddDays(-1);
        public int? OCRequestId { get; set; }
        public bool Confirmed { get; set; }
        public bool Paid { get; set; }


        public class CompanyCompanyTypeWebDataFormatter : AbstractModelEditor<CompanyCompanyType, JobChINModule>
        {
            public CompanyCompanyTypeWebDataFormatter()
            {
                Setup(cfg =>
                {
                    cfg.SetName("Název", x => GetName(x), namePropertyHide: true);
                    cfg.SetId(x => x.CompanyCompanyTypeId);
                });

                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Typ", x => x.CompanyTypeId)
                        .SetDataType(x => x.SingleValuePicker(() => Module.CompanyTypeController.GetAll().Select(y => EnumerablePickerValue.From<int, string>(y.CompanyTypeId, y.Name))));
                    cfg.AddField("Počet inzerátů", x => x.NumberOfStudentsRevealed)
                        .SetDescription("Pokud není vyplněno, nastaví se na 'neomezeno'")
                        .SetDataType(x => x.Number());
                    cfg.AddField("Počet odrytí studentů", x => x.NumberOfWorkPosition)
                        .SetDescription("Pokud není vyplněno, nastaví se na 'neomezeno'")
                        .SetDataType(x => x.Number());
                    cfg.AddField("Vyhledávání v databázi studentů", x => x.DatabaseSearch)
                        .SetDataType(x => x.Boolean());
                    cfg.AddField("Prezentační profil zaměstnavatele", x => x.CompanyPresentation)
                        .SetDataType(x => x.Boolean());
                    cfg.AddField("Aktivní od", x => x.ActiveFrom)
                        .SetDataType(x => x.DateTime(DateTimeFieldEnum.Date));
                    cfg.AddField("Aktivní do", x => x.ActiveTo)
                        .SetDescription("Platnost tarifu včetně daného dne")
                        .SetDataType(x => x.DateTime(DateTimeFieldEnum.Date));
                    cfg.AddField("Potvrzeno správcem", x => x.Confirmed)
                        .SetDescription("Pokud je pole zaškrnuto, tak je typ účtu aktivní, i když není ještě zaplacený")
                        .SetDataType(x => x.Boolean());
                    
                    if (CurrentRecord.OCRequestId.HasValue)
                    {
                        cfg.AddField("Zaplaceno", x => x.Paid)
                            .SetDescription("Pole je vyplněno automaticky podle stavu objednávky v OC")
                            .SerializeAs(x => x ? "Ano" : "Ne")
                            .SetDataType(x => x.ReadOnlyText());
                    }
                    else
                    {
                        cfg.AddField("Zaplaceno", x => x.Paid)
                            .SetDataType(x => x.Boolean());
                    }
                });
            }

            string GetName(CompanyCompanyType model)
            {
                string name = model.CompanyTypeId == 0 ? string.Empty : Module.CompanyTypeController.GetById(model.CompanyTypeId).Name;
                return $"{name} ({model.ActiveFrom.ToShortDateString()} - {model.ActiveTo.ToShortDateString()})";
            }
        }

        public class CompanyCompanyTypeValidator : AbstractValidator<CompanyCompanyType>
        {
            public CompanyCompanyTypeValidator()
            {
                RuleFor(x => x.ActiveTo)
                    .GreaterThan(x => x.ActiveFrom);
            }
        }
    }
}