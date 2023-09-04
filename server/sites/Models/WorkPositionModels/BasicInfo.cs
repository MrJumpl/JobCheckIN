using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;
using Mlok.Web.Sites.JobChIN.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Models.WorkPositionModels
{
    [Validator(typeof(BasicInfoValidator))]
    [ModelEditor(typeof(BasicInfoWebDataFormatter))]
    public class BasicInfo : IAnonymizable
    {
        public DateTime Expiration { get; set; } = DateTime.Now;
        public DateTime Publication { get; set; } = DateTime.Now;
        public DateTime? JobBeginning { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public Remote Remote { get; set; }
        public string LocationId { get; set; }
        public int? BranchId { get; set; }
        public IEnumerable<int> ContractTypes { get; set; }
        public IEnumerable<int> Users { get; set; }
        public bool Public { get; set; }

        // only for webdata use
        public int? CurrentCompanyId { get; set; }
        // from databse
        public string CurrentLocationId{ get; set; }

        public void AnonymizeData()
        {
            Name = string.Empty;
            LocationId = null;
            BranchId = null;
        }


        public class BasicInfoWebDataFormatter : AbstractModelEditor<BasicInfo, JobChINModule>
        {
            public BasicInfoWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Název pracovní pozice", x => x.Name);
                    cfg.AddField("Jazyk inzerátu", x => x.Language)
                        .SetDataType(x => x.SingleValuePicker(() => Module.Site.SiteNode.GetSupportedLanguages().Select(y => EnumerablePickerValue.From(y.TwoLetterISOLanguageName, y.DisplayName))));
                    cfg.AddField("Zveřejnit inzerát od", x => x.Publication)
                        .SetDataType(x => x.DateTime(DateTimeFieldEnum.Date));
                    cfg.AddField("Zveřejnit inzerát do", x => x.Expiration)
                        .SetDataType(x => x.DateTime(DateTimeFieldEnum.Date));
                    cfg.AddField("Nástup do práce", x => x.JobBeginning)
                        .SetDataType(x => x.DateTime(DateTimeFieldEnum.Date));
                    cfg.AddField("Typ pracovního úvazku", x => x.ContractTypes)
                        .SetDataType(x => x.MultipleValuePicker(() => Module.ContractTypeController.GetPicker()));
                    cfg.AddField("Práce z domova", x => x.Remote)
                        .SetDataType(x => x.Toggle<Remote>());
                    cfg.AddField("Místo pracovní pozice", x => x.LocationId)
                        .SetDataType(x => x.SingleValuePicker(() => Module.LocalAdministrativeUnitsController.GetPicker()));
                    if (CurrentRecord.CurrentCompanyId.HasValue)
                    {
                        cfg.AddField("Pobočka", x => x.BranchId)
                            .SetDataType(x => x.SingleValuePicker(() => Module.CompanyService.GetCompanyBranchPicker(CurrentRecord.CurrentCompanyId)));
                    }
                });
            }
        }

        public class BasicInfoValidator : AbstractValidator<BasicInfo>
        {
            public BasicInfoValidator()
            {
                RuleFor(x => x.Expiration)
                    .GreaterThan(bi => bi.Publication)
                    .WithMessage(x => this.Localize("Pole 'Zveřejnění do' musí být později než pole 'Zveřejnit od'", "")); // TODO: translate

                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MaximumLength(WebDataConstants.MaximumWorkPositionNameLength)
                    .WithName(x => this.Localize("Název pracovní pozice", "Job name"));

                RuleFor(x => x.ContractTypes)
                    .ListUniqueness(this.Localize("Typ pracovního úvazku", "")); // TODO: translate
            }
        }
    }
}