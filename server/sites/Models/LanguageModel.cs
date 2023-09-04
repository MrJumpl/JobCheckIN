using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using System;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Models
{
    [Validator(typeof(LanguageModelValidator))]
    [ModelEditor(typeof(LanguageWebDataFormatter<LanguageModel>))]
    public class LanguageModel
    {
        public int? LanguageId { get; set; }
        public LanguageSkill? Skill { get; set; }


        public class LanguageWebDataFormatter<T> : AbstractModelEditor<T, JobChINModule>
            where T : LanguageModel
        {
            public LanguageWebDataFormatter()
            {
                Setup(cfg =>
                {
                    cfg.SetName("Název", x => GetName(x));
                });

                SetupSimpleModelEditor(SimpleModelEditor);
            }

            protected virtual void SimpleModelEditor(SimpleModelEditorConfig<T> cfg)
            {
                cfg.AddField("Jazyk", x => x.LanguageId)
                    .SetDataType(x => x.SingleValuePicker(() => Module.LanguageController.GetPicker()));
                cfg.AddField("Úroveň", x => x.Skill)
                    .SetDataType(x => x.SingleValuePicker(() =>
                        Enum.GetValues(typeof(LanguageSkill)).Cast<LanguageSkill>().Select(y => EnumerablePickerValue.From<LanguageSkill?, string>(y, EnumUtils.GetDisplayName(y)))));
            }

            protected virtual string GetName(T model)
            {
                bool hasLang = model.LanguageId.HasValue;
                bool hasSkill = model.Skill.HasValue;
                return (hasLang ? Module.LanguageController.GetById(model.LanguageId.Value).Name.ToString() : string.Empty) +
                    (hasLang && hasSkill ? " - " : string.Empty) +
                    (hasSkill ? EnumUtils.GetDisplayName(model.Skill.Value) : string.Empty);
            }
        }

        public class LanguageModelValidator : AbstractValidator<LanguageModel>
        {
            public LanguageModelValidator()
            {
                RuleFor(x => x.LanguageId)
                    .NotEmpty()
                    .WithName(x => this.Localize("Jazyk", "Language"));

                RuleFor(x => x.Skill)
                    .NotEmpty()
                    .WithName(x => this.Localize("Úroveň", "Level"));
            }
        }
    }
}