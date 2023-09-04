using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;
using Mlok.Web.Sites.JobChIN.Utils;

namespace Mlok.Web.Sites.JobChIN.Models.StudentModels
{
    [Validator(typeof(ContactValidator))]
    [ModelEditor(typeof(ContactWebDataFormatter))]
    public class Contact : ICompleteness, IAnonymizable
    {
        public string PrivateEmail { get; set; }
        public string Phone { get; set; }
        public string Linkedin { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }

        public int Completeness()
        {
            int count = 0;
            if (!string.IsNullOrWhiteSpace(PrivateEmail))
                count++;
            if (!string.IsNullOrWhiteSpace(Phone))
                count++;
            if (!string.IsNullOrWhiteSpace(Linkedin))
                count++;
            if (!string.IsNullOrWhiteSpace(Facebook))
                count++;
            if (!string.IsNullOrWhiteSpace(Twitter))
                count++;
            return count;
        }

        public int FullyCompleted() => 5;

        public void AnonymizeData()
        {
            PrivateEmail = null;
            Phone = null;
            Linkedin = null;
            Facebook = null;
            Twitter = null;
        }


        public class ContactWebDataFormatter : AbstractModelEditor<Contact, JobChINModule>
        {
            public ContactWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("E-mail", x => x.PrivateEmail);
                    cfg.AddField("Telefon", x => x.Phone);
                    cfg.AddField("Linkedin", x => x.Linkedin)
                        .SetDescription("Část adresy nacházející se za 'www.linkedin.com/in/'"); ;
                    cfg.AddField("Facebook", x => x.Facebook)
                        .SetDescription("Část adresy nacházející se za 'www.facebook.com/'"); ;
                    cfg.AddField("Twitter", x => x.Twitter)
                        .SetDescription("Část adresy nacházející se za 'www.twitter.com/'"); ;
                });
            }
        }

        public class ContactValidator : AbstractValidator<Contact>
        {
            public ContactValidator()
            {
                RuleFor(x => x.PrivateEmail)
                    .MaximumLength(WebDataConstants.MaximumNotificationEmailLenght)
                    .EmailAddress()
                    .WithName(_ => this.Localize("E-mail", "Email"));
                RuleFor(x => x.Phone)
                    .MaximumLength(WebDataConstants.MaximumContactLenght)
                    .WithName(_ => this.Localize("Telefon", "Phone"));
                RuleFor(x => x.Phone)
                    .ValidatePhone(this.Localize("Telefon", "Phone"));
                RuleFor(x => x.Linkedin)
                    .MaximumLength(WebDataConstants.MaximumSocialMediaLenght);
                RuleFor(x => x.Facebook)
                    .MaximumLength(WebDataConstants.MaximumSocialMediaLenght);
                RuleFor(x => x.Twitter)
                    .MaximumLength(WebDataConstants.MaximumSocialMediaLenght);
            }
        }
    }
}