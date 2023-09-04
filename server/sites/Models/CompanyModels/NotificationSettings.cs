using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;
using Mlok.Web.Sites.JobChIN.Utils;

namespace Mlok.Web.Sites.JobChIN.Models.CompanyModels
{
    [Validator(typeof(NotificationSettingsDtoValidator))]
    [ModelEditor(typeof(NotificationSettingsWebDataFormatter))]
    public class NotificationSettings : IAnonymizable
    {
        public NotificationFrequency NotificationFrequency { get; set; }
        public string NotificationEmail { get; set; }

        public void AnonymizeData()
        {
            NotificationEmail = null;
        }


        public class NotificationSettingsWebDataFormatter : AbstractModelEditor<NotificationSettings, JobChINModule>
        {
            public NotificationSettingsWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Jak často chcete dostavát upozornění?", x => x.NotificationFrequency)
                        .SetDataType(x => x.SingleValuePicker(() => NotificationFrequencyUtils.GetPicker()));
                    cfg.AddField("E-mail pro notifikace", x => x.NotificationEmail);
                });
            }
        }

        public class NotificationSettingsDtoValidator : AbstractValidator<NotificationSettings>
        {
            public NotificationSettingsDtoValidator()
            {
                RuleFor(x => x.NotificationFrequency)
                    .NotEmpty()
                    .WithName(_ => this.Localize("Jak často chcete dostavát upozornění?", "")); // TODO: translate

                RuleFor(x => x.NotificationEmail)
                    .EmailAddress()
                    .MaximumLength(WebDataConstants.MaximumNotificationEmailLenght)
                    .WithName(_ => this.Localize("E-mail pro notifikace", "E-mail for notifications"));
            }
        }
    }
}