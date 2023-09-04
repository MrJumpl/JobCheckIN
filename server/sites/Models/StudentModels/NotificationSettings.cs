using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;
using Mlok.Web.Sites.JobChIN.Utils;

namespace Mlok.Web.Sites.JobChIN.Models.StudentModels
{
    [Validator(typeof(NotificationSettingsValidator))]
    [ModelEditor(typeof(NotificationSettingsWebDataFormatter))]
    public class NotificationSettings : IAnonymizable
    {
        public NotificationFrequency NotificationFrequency { get; set; }
        public string NotificationEmail { get; set; }
        public NotificationFrequency WorkPositionNotificationFrequency { get; set; }

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
                    cfg.AddField("Jak často chcete dostávat upozornění o nových pracovních pozicích na e-mail?", x => x.WorkPositionNotificationFrequency)
                        .SetDataType(x => x.SingleValuePicker(() => NotificationFrequencyUtils.GetPicker()));
                });
            }
        }

        public class NotificationSettingsValidator : AbstractValidator<NotificationSettings>
        {
            public NotificationSettingsValidator()
            {
                RuleFor(x => x.NotificationFrequency)
                    .IsInEnum()
                    .WithMessage(_ => this.Localize("Pole 'Jak často chcete dostavát upozornění?' musí být vybráno.", "")); // TODO: translate
                RuleFor(x => x.NotificationEmail)
                    .EmailAddress()
                    .MaximumLength(WebDataConstants.MaximumNotificationEmailLenght)
                    .WithName(_ => this.Localize("E-mail", "Email"));
                RuleFor(x => x.WorkPositionNotificationFrequency)
                    .IsInEnum()
                    .WithName(_ => this.Localize("Pole 'Jak často chcete dostávat upozornění o nových pracovních pozicích na e-mail?' musí být vybráno.", "")); // TODO: translate
            }
        }
    }
}