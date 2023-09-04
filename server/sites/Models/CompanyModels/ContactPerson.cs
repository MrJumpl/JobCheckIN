using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;
using Mlok.Web.Sites.JobChIN.Utils;
using PhoneNumbers;

namespace Mlok.Web.Sites.JobChIN.Models.CompanyModels
{
    [Validator(typeof(ContactPersonDtoValidator))]
    [ModelEditor(typeof(ContactPersonWebDataFormatter))]
    public class ContactPerson : IAnonymizable
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }

        public void AnonymizeData()
        {
            Firstname = "";
            Surname = "";
            Phone = "";
        }


        public class ContactPersonWebDataFormatter : AbstractModelEditor<ContactPerson, JobChINModule>
        {
            public ContactPersonWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Jméno", x => x.Firstname);
                    cfg.AddField("Příjmení", x => x.Surname);
                    cfg.AddField("Telefoní číslo", x => x.Phone);
                });
            }
        }

        public class ContactPersonDtoValidator : AbstractValidator<ContactPerson>
        {
            public ContactPersonDtoValidator()
            {
                RuleFor(x => x.Firstname)
                    .NotEmpty()
                    .MaximumLength(WebDataConstants.MaximumContactLenght)
                    .WithName(_ => this.Localize("Jméno", "Firstname"));

                RuleFor(x => x.Surname)
                    .NotEmpty()
                    .MaximumLength(WebDataConstants.MaximumContactLenght)
                    .WithName(_ => this.Localize("Příjmení", "Surname"));

                RuleFor(x => x.Phone)
                    .NotEmpty()
                    .MaximumLength(WebDataConstants.MaximumContactLenght)
                    .WithName(_ => this.Localize("Telefonní číslo", "Phone number"));

                RuleFor(x => x.Phone)
                    .ValidatePhone(this.Localize("Telefonní číslo", "Phone number"));
            }
        }
    }
}