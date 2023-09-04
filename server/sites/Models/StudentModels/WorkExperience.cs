using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;
using System;

namespace Mlok.Web.Sites.JobChIN.Models.StudentModels
{
    [Validator(typeof(WorkExperienceValidator))]
    [ModelEditor(typeof(WorkExperienceWebDataFormatter))]
    public class WorkExperience
    {
        public int WorkExperienceId { get; set; }
        public string Position { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public int AreaOfInterestId { get; set; }
        public DateTime From { get; set; } = DateTime.Now;
        public DateTime? To { get; set; }
        public bool Paid { get; set; }
        public int ContractTypeId { get; set; }
        public string ContactPerson { get; set; }
        public string Contact { get; set; }
        public string Description { get; set; }


        public class WorkExperienceWebDataFormatter : AbstractModelEditor<WorkExperience, JobChINModule>
        {
            public WorkExperienceWebDataFormatter()
            {
                Setup(cfg => {
                    cfg.SetId(x => x.WorkExperienceId);
                    cfg.SetName("Název", x => x.GetTitle());
                });

                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Pozice ", x => x.Position);
                    cfg.AddField("Název společnosti", x => x.CompanyName);
                    cfg.AddField("Místo ", x => x.City);
                    cfg.AddField("Práce byla v oblasti", x => x.AreaOfInterestId)
                        .SerializeAs(x => x == default(int) ? null : (int?)x, x => x ?? default(int))
                        .SetDataType(x => x.SingleValuePicker(() => Module.AreaOfInterestController.GetPicker()));
                    cfg.AddField("Pracoval/a jsem tam od", x => x.From)
                        .SetDataType(x => x.DateTime(DateTimeFieldEnum.Date));
                    cfg.AddField("Pracoval/a jsem tam do", x => x.To)
                        .SetDataType(x => x.DateTime(DateTimeFieldEnum.Date));
                    cfg.AddField("Byla placená", x => x.Paid)
                        .SetDataType(x => x.Boolean());
                    cfg.AddField("Typ pracovního úvazku", x => x.ContractTypeId)
                        .SerializeAs(x => x == default(int) ? null : (int?)x, x => x ?? default(int))
                        .SetDataType(x => x.SingleValuePicker(() => Module.ContractTypeController.GetPicker()));
                    cfg.AddField("Popište, čemu jste se v práci věnovali", x => x.Description)
                        .SetDataType(x => x.Rte());
                    cfg.AddField("Kontaktní osoba", x => x.ContactPerson);
                    cfg.AddField("Telefon nebo e-mail", x => x.Contact);
                });
            }
        }

        public class WorkExperienceValidator : AbstractValidator<WorkExperience>
        {
            public WorkExperienceValidator()
            {
                RuleFor(x => x.Position)
                    .NotEmpty()
                    .MaximumLength(WebDataConstants.MaximumWorkExperienceLength)
                    .WithName(_ => this.Localize("Pozice", "Position"));
                RuleFor(x => x.CompanyName)
                    .NotEmpty()
                    .MaximumLength(WebDataConstants.MaximumWorkExperienceLength)
                    .WithName(_ => this.Localize("Název společnosti", "Company name"));
                RuleFor(x => x.City)
                    .NotEmpty()
                    .MaximumLength(WebDataConstants.MaximumStudentCityLength)
                    .WithName(_ => this.Localize("Místo", "")); // TODO: translate
                RuleFor(x => x.AreaOfInterestId)
                    .GreaterThan(default(int))
                    .WithMessage(_ => this.Localize("Pole 'Práce byla v oblasti' musí být vyplněno", "")); // TODO: translate
                RuleFor(x => x.From)
                    .GreaterThan(DateTime.MinValue)
                    .LessThan(DateTime.Now)
                    .WithMessage(_ => this.Localize("Pole 'Pracoval/a jsem tam od' není vyplněno správně", "")); // TODO: translate
                RuleFor(x => x.To)
                    .GreaterThan(x => x.From)
                    .When(x => x.To.HasValue)
                    .WithMessage(_ => this.Localize("Pole 'Pracoval/a jsem tam do' není vyplněno správně", "")); // TODO: translate
                RuleFor(x => x.ContractTypeId)
                    .GreaterThan(default(int))
                    .WithMessage(_ => this.Localize("Pole 'Typ pracovního úvazku' musí být vyplněno", "")); // TODO: translate
                RuleFor(x => x.Description)
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Popište, čemu jste se v práci věnovali", "")); // TODO: translate
                RuleFor(x => x.ContactPerson)
                    .MaximumLength(WebDataConstants.MaximumWorkExperienceLength)
                    .WithName(_ => this.Localize("Kontaktní osoba", " Contact person"));
                RuleFor(x => x.Contact)
                    .MaximumLength(WebDataConstants.MaximumWorkExperienceLength)
                    .WithName(_ => this.Localize("Telefon nebo e-mail", "Phone or email"));

                RuleFor(x => x.Contact)
                    .NotEmpty()
                    .When(x => !string.IsNullOrWhiteSpace(x.ContactPerson))
                    .WithMessage(_ => this.Localize(
                        "Pole 'Kontaktní osoba' musí být vyplněno, pokud je vyplněno pole 'Telefon nebo e-mail'",
                        "The 'Contact person' field must be filled in if the 'Phone or email' field is filled in"));
                RuleFor(x => x.ContactPerson)
                    .NotEmpty()
                    .When(x => !string.IsNullOrWhiteSpace(x.Contact))
                    .WithMessage(_ => this.Localize(
                        "Pole 'Telefon nebo e-mail' musí být vyplněno, pokud je vyplněno pole 'Kontaktní osoba'",
                        "The 'Phone or email' field must be filled in if the 'Contact person' field is filled in"));
            }
        }

        string GetTitle()
        {
            string result = CompanyName ?? "";
            if (!string.IsNullOrWhiteSpace(CompanyName) && !string.IsNullOrWhiteSpace(Position))
                result += " - ";
            result += Position;

            return result;
        }
    }
}