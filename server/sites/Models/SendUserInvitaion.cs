using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;

namespace Mlok.Web.Sites.JobChIN.Models
{
    [Validator(typeof(SendUserInvitaionValidator))]
    [ModelEditor(typeof(SendUserInvitaionWebDataFormatter))]
    public class SendUserInvitaion
    {
        public string Email { get; set; }
        public Role Role { get; set; }
        public bool DeleteMember { get; set; }


        public class SendUserInvitaionWebDataFormatter : AbstractModelEditor<SendUserInvitaion, JobChINModule>
        {
            public SendUserInvitaionWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Email", x => x.Email);

                    cfg.AddField("Role", x => x.Role)
                        .SetDataType(x => x.Toggle<Role>());
                });
            }
        }

        public class SendUserInvitaionValidator : AbstractValidator<SendUserInvitaion>
        {
            public SendUserInvitaionValidator()
            {
                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .MaximumLength(250);

                RuleFor(x => x.Role)
                    .IsInEnum();

                RuleFor(x => x.Role)
                    .Equal(Role.CompanyAdmin)
                    .When(x => x.DeleteMember)
                    .WithMessage(this.Localize("Nelze předat účet uživateli, který nebude správcem firmy", "")); // TODO: translate
            }
        }

    }
}