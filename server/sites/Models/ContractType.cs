using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;

namespace Mlok.Web.Sites.JobChIN.Models
{
    [Validator(typeof(ContractTypeValidator))]
    [ModelEditor(typeof(ContractTypeWebDataFormatter))]
    public class ContractType
    {
        public int ContractTypeId { get; set; }
        public WebDataI18N<string> Name { get; set; }
        public bool Visible { get; set; }
        public class ContractTypeWebDataFormatter : AbstractModelEditor<ContractType, JobChINModule>
        {
            public ContractTypeWebDataFormatter()
            {
                Setup(cfg =>
                {
                    cfg.SetName("Název", x => x.Name, namePropertyHide: true);
                });

                SetupOverview(listviewCfg => { });

                SetupModelEditor(detailCfg =>
                {
                    detailCfg.AddTab("Metadata", tabConfig =>
                    {
                        tabConfig.AddField("Název", x => x.Name);
                        tabConfig.AddField("Viditelné", x => x.Visible)
                            .SetDataType(x => x.Number());
                    });
                });
            }
        }

        public class ContractTypeValidator : AbstractValidator<ContractType>
        {
            public ContractTypeValidator()
            {
                RuleFor(x => x.Name)
                    .RequireLanguages(WebDataConstants.RequiredLanguages)
                    .MaximumLength(WebDataConstants.MaximumNameLength);
            }
        }

    }
}