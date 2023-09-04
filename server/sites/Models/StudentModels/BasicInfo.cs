using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;
using Mlok.Web.Sites.JobChIN.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Models.StudentModels
{
    [Validator(typeof(BasicInfoValidator))]
    [ModelEditor(typeof(BasicInfoWebDataFormatter))]
    public class BasicInfo : ICompleteness, IAnonymizable
    {
        private DateTime? _preferredJobBeginning;

        public bool ActiveDriver { get; set; }
        public bool DrivingLicense { get; set; }
        public string CareerVision { get; set; }
        public string Presentation { get; set; }
        public IEnumerable<int> AreaOfInterests { get; set; }
        public IEnumerable<int> SecondaryAreaOfInterests { get; set; }
        public IEnumerable<int> ContractTypes { get; set; }
        public string PreferedLocationId { get; set; }
        public bool WillingToMove { get; set; }
        public DateTime? PreferredJobBeginning { get => _preferredJobBeginning?.Date; set => _preferredJobBeginning = value; }

        public int Completeness()
        {
            int count = 0;
            if (!string.IsNullOrWhiteSpace(CareerVision))
                count++;
            if (!string.IsNullOrWhiteSpace(Presentation))
                count++;
            if (AreaOfInterests?.Any() ?? false)
                count++;
            if (ContractTypes?.Any() ?? false)
                count++;
            if (PreferredJobBeginning.HasValue)
                count++;
            return count;
        }

        public int FullyCompleted() => 5;

        public void AnonymizeData()
        {
            CareerVision = null;
            Presentation = null;
            PreferedLocationId = null;
            PreferredJobBeginning = null;
        }


        public class BasicInfoWebDataFormatter : AbstractModelEditor<BasicInfo, JobChINModule>
        {
            public BasicInfoWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Řidičský průkaz", x => x.DrivingLicense)
                        .SetDataType(x => x.Boolean());
                    cfg.AddField("Aktivní řidič", x => x.ActiveDriver)
                        .SetDataType(x => x.Boolean());
                    cfg.AddField("Kariérní vize", x => x.CareerVision)
                        .SetDataType(x => x.Rte());
                    cfg.AddField("Kdo jsem", x => x.Presentation)
                        .SetDataType(x => x.Rte());
                    cfg.AddField("Primární oblasti zájmu", x => x.AreaOfInterests)
                        .SetDataType(x => x.MultipleValuePicker(() => Module.AreaOfInterestController.GetPicker()));
                    cfg.AddField("Sekundární oblasti zájmu", x => x.SecondaryAreaOfInterests)
                        .SetDataType(x => x.MultipleValuePicker(() => Module.AreaOfInterestController.GetPicker()));
                    cfg.AddField("Typ pracovního úvazku", x => x.ContractTypes)
                        .SetDataType(x => x.MultipleValuePicker(() => Module.ContractTypeController.GetPicker()));
                    cfg.AddField("Ochoten přestěhovat se", x => x.WillingToMove)
                        .SetDataType(x => x.Boolean());
                    cfg.AddField("Hledá práci v", x => x.PreferedLocationId)
                        .SetDataType(x => x.SingleValuePicker(() => Module.LocalAdministrativeUnitsController.GetPicker()));
                    cfg.AddField("Mohu začít pracovat od", x => x.PreferredJobBeginning)
                        .SetDataType(x => x.DateTime(DateTimeFieldEnum.Date));
                });
            }
        }

        public class BasicInfoValidator : AbstractValidator<BasicInfo>
        {
            public BasicInfoValidator()
            {
                RuleFor(x => x.DrivingLicense)
                    .Must(x => x)
                    .When(x => x.ActiveDriver)
                    .WithMessage(_ => this.Localize(
                        "Pole 'Řidičský průkaz' musí být zaškrtnuté, pokud je pole 'aktivní řidič' zašrknuto.",
                        "The 'Driving licence' field must be checked if the 'active driver' field is ticked."));

                RuleFor(x => x.PreferedLocationId)
                    .MaximumLength(6)
                    .WithName(_ => this.Localize("Hledá práci v", "")); // TODO: translate

                RuleFor(x => x.CareerVision)
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Kariérní vize", "Career vision"));

                RuleFor(x => x.Presentation)
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Kdo jsem", "")); // TODO: translate

                RuleFor(x => x.AreaOfInterests)
                    .ListUniqueness(this.Localize("Primární oblasti zájmu", "")); // TODO: translate

                RuleFor(x => x.AreaOfInterests)
                    .Must(x => x == null || x.Count() <= 3)
                    .WithMessage(_ => this.Localize(
                        "Pole 'Primární oblasti zájmu' může obsahovat maximálně 3 oblasti zájmu.",
                        "")); // TODO: translate

                RuleFor(x => x.SecondaryAreaOfInterests)
                    .ListUniqueness(this.Localize("Sekundární oblasti zájmu", "")); // TODO: translate

                RuleFor(x => x.SecondaryAreaOfInterests)
                    .Must((model, secondary) => model.AreaOfInterests == null || secondary == null || !secondary.Intersect(model.AreaOfInterests).Any())
                    .WithMessage(_ => this.Localize(
                        "Pole 'Sekundární oblasti zájmu' nesmí obsahovat primánrní oblasti zájmu.",
                        "")); // TODO: translate

                RuleFor(x => x.ContractTypes)
                    .ListUniqueness(this.Localize("Typ pracovního úvazku", "")); // TODO: translate

                RuleFor(x => x.PreferedLocationId)
                    .Must(x => !string.IsNullOrWhiteSpace(x))
                    .When(x => !x.WillingToMove)
                    .WithMessage(_ => this.Localize(
                        "Pole 'Hledá práci v' musí být vyplněno, pokud nejste flexibilní.",
                        "")); // TODO: translate
            }
        }
    }
}