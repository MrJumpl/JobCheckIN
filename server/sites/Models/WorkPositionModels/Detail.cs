using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;

namespace Mlok.Web.Sites.JobChIN.Models.WorkPositionModels
{
    [Validator(typeof(DetailValidator))]
    [ModelEditor(typeof(DetailWebDataFormatter))]
    public class Detail : IAnonymizable
    {
        public string CompanyDescription { get; set; }
        public string Description { get; set; }
        public string Requesting { get; set; }
        public string Offering { get; set; }
        public string CustomField1Name { get; set; }
        public string CustomField2Name { get; set; }
        public string CustomField3Name { get; set; }
        public string CustomField1Value { get; set; }
        public string CustomField2Value { get; set; }
        public string CustomField3Value { get; set; }

        public void AnonymizeData()
        {
            CompanyDescription = null;
            Description = string.Empty;
            Requesting = string.Empty;
            Offering = null;
            CustomField1Name = null;
            CustomField2Name = null;
            CustomField3Name = null;
            CustomField1Value = null;
            CustomField2Value = null;
            CustomField3Value = null;
        }


        public class DetailWebDataFormatter : AbstractModelEditor<Detail, JobChINModule>
        {
            public DetailWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Představení společnosti", x => x.CompanyDescription)
                        .SetDataType(x => x.MultilineText());
                    cfg.AddField("Popis pracovní pozice", x => x.Description)
                        .SetDataType(x => x.Rte());
                    cfg.AddField("Požadujeme ", x => x.Requesting)
                        .SetDataType(x => x.Rte());
                    cfg.AddField("Nabízíme", x => x.Offering)
                        .SetDataType(x => x.Rte());
                    cfg.AddField("Název první sekce", x => x.CustomField1Name);
                    cfg.AddField("Popis první sekce", x => x.CustomField1Value)
                        .SetDataType(x => x.Rte());
                    cfg.AddField("Název druhé sekce", x => x.CustomField2Name);
                    cfg.AddField("Popis druhé sekce", x => x.CustomField2Value)
                        .SetDataType(x => x.Rte());
                    cfg.AddField("Název třetí sekce", x => x.CustomField3Name);
                    cfg.AddField("Popis třetí sekce", x => x.CustomField3Value)
                        .SetDataType(x => x.Rte());

                });
            }
        }

        public class DetailValidator : AbstractValidator<Detail>
        {
            public DetailValidator()
            {
                RuleFor(x => x.CompanyDescription)
                    .MaximumLength(WebDataConstants.MaximumDescLength)
                    .WithName(_ => this.Localize("Představení společnosti", "Company introduction"));

                RuleFor(x => x.Description)
                    .NotEmpty()
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Popis pracovní pozice", "Job description"));

                RuleFor(x => x.Requesting)
                    .NotEmpty()
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Požadujeme", "Requesting"));

                RuleFor(x => x.Offering)
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Nabízíme", "Offering"));

                RuleFor(x => x.CustomField1Name)
                    .MaximumLength(WebDataConstants.MaximumCustomFieldNameLength)
                    .WithName(_ => this.Localize("Název první sekce", "Name of the first section"));

                RuleFor(x => x.CustomField1Value)
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Popis první sekce", "Description of the first section"));

                RuleFor(x => x.CustomField2Name)
                    .MaximumLength(WebDataConstants.MaximumCustomFieldNameLength)
                    .WithName(_ => this.Localize("Název druhé sekce", "Name of the second section"));

                RuleFor(x => x.CustomField2Value)
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Popis druhé sekce", "Description of the second section"));

                RuleFor(x => x.CustomField3Name)
                    .MaximumLength(WebDataConstants.MaximumCustomFieldNameLength)
                    .WithName(_ => this.Localize("Název třetí sekce", "Name of the third section"));

                RuleFor(x => x.CustomField3Value)
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Popis třetí sekce", "Description of the third section"));
            }
        }
    }
}