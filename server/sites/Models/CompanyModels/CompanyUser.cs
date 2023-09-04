using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Umbraco.Core;
using Umbraco.Web.PublishedCache;

namespace Mlok.Web.Sites.JobChIN.Models.CompanyModels
{
    [Validator(typeof(CompanyUserValidator))]
    [ModelEditor(typeof(CompanyUserWebDataFormatter))]
    public class CompanyUser : IUser
    {
        MemberPublishedContent _member;

        public int MemberId { get; set; }
        public int CompanyId { get; set; }
        public Role Role { get; set; }
        public MemberPublishedContent Member {
            get => _member;
            set
            {
                MemberId = value?.Id ?? 0;
                _member = value;
            }
        }
        public ContactPerson ContactPerson { get; set; }
        public NotificationSettings NotificationSettings { get; set; }

        public string FullName => $"{ContactPerson.Firstname} {ContactPerson.Surname}";

        public string EmailForNotiofications()
        {
            if (NotificationSettings?.NotificationEmail.IsNullOrWhiteSpace() ?? true)
                return Member.Email;
            return NotificationSettings.NotificationEmail;
        }

        public class CompanyUserWebDataFormatter : AbstractModelEditor<CompanyUser, JobChINModule>
        {
            public CompanyUserWebDataFormatter()
            {
                Setup(cfg =>
                {
                    cfg.SetName("Název", x => x.FullName, namePropertyHide: true);
                    cfg.SetId(x => x.MemberId);
                });

                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Role", x => x.Role)
                        .SetDataType(x => x.Toggle<Role>());
                    cfg.AddField("Osobní údaje", x => x.ContactPerson);
                    cfg.AddField("Notifikace", x => x.NotificationSettings);
                });
            }
        }

        public class CompanyUserValidator : AbstractValidator<CompanyUser>
        {
            public CompanyUserValidator()
            {
                RuleFor(x => x.MemberId)
                    .GreaterThan(0)
                    .WithMessage(this.Localize(
                        "Nezle vytvořit takto nového uživatele. Musíte mu poslat pozvánku, aby si vytvořil účet sám.",
                        "Cannot create new user this way. You have to send invitaion to create an account himself"));
            }
        }
    }
}