using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;
using System.Collections.Generic;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Models.StudentModels
{
    [Validator(typeof(WorkExperiencesValidator))]
    [ModelEditor(typeof(WorkExperiencesWebDataFormatter))]
    public class WorkExperiences : ICompleteness, IAnonymizable
    {
        public IEnumerable<WorkExperience> Experiences { get; set; }
        public string CareerPortfolio { get; set; }

        public int Completeness()
        {
            int count = 0;
            if (!string.IsNullOrWhiteSpace(CareerPortfolio))
                count++;
            return count;
        }

        public int FullyCompleted() => 1;

        public void AnonymizeData()
        {
            CareerPortfolio = null;
            Experiences = Enumerable.Empty<WorkExperience>();
        }

        public class WorkExperiencesWebDataFormatter : AbstractModelEditor<WorkExperiences, JobChINModule>
        {
            public WorkExperiencesWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Pracovní zkušenosti", x => x.Experiences);

                    cfg.AddField("Kariérní portfolio", x => x.CareerPortfolio)
                        .SetDataType(x => x.Rte());
                });
            }
        }

        public class WorkExperiencesValidator : AbstractValidator<WorkExperiences>
        {
            public WorkExperiencesValidator()
            {
                RuleFor(x => x.CareerPortfolio)
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Kariérní vize", "Career portfolio"));
            }
        }
    }
}