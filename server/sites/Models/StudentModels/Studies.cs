using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;
using System.Collections.Generic;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Models.StudentModels
{
    [Validator(typeof(StudiesValidator))]
    [ModelEditor(typeof(StudiesWebDataFormatter))]
    public class Studies : ICompleteness, IAnonymizable
    {
        public IEnumerable<OtherStudy> Others { get; set; }
        public string AdditionalEducation { get; set; }

        public int Completeness()
        {
            int count = 0;
            if (!string.IsNullOrWhiteSpace(AdditionalEducation))
                count++;
            return count;
        }

        public int FullyCompleted() => 1;

        public void AnonymizeData()
        {
            AdditionalEducation = null;
            Others = Enumerable.Empty<OtherStudy>();
        }


        public class StudiesWebDataFormatter : AbstractModelEditor<Studies, JobChINModule>
        {
            public StudiesWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Studium na jiné VŠ", x => x.Others);

                    cfg.AddField("Kurzy", x => x.AdditionalEducation)
                        .SetDataType(x => x.Rte());
                });
            }
        }

        public class StudiesValidator : AbstractValidator<Studies>
        {
            public StudiesValidator()
            {
                RuleFor(x => x.AdditionalEducation)
                    .MaximumLength(WebDataConstants.MaximumRteLength)
                    .WithName(_ => this.Localize("Kurzy", "Courses"));
            }
        }
    }
}