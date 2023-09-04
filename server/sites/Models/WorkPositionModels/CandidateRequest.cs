using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;

namespace Mlok.Web.Sites.JobChIN.Models.WorkPositionModels
{
    [Validator(typeof(CandidateRequestValidator))]
    [ModelEditor(typeof(CandidateRequestWebDataFormatter))]
    public class CandidateRequest : IAnonymizable
    {
        public bool CoverLetter { get; set; }
        public string AdditionalQuestions { get; set; }

        public void AnonymizeData()
        {
            AdditionalQuestions = string.IsNullOrEmpty(AdditionalQuestions) ? null : "(deleted)";
        }


        public class CandidateRequestWebDataFormatter : AbstractModelEditor<CandidateRequest, JobChINModule>
        {
            public CandidateRequestWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Motivační dopis", x => x.CoverLetter)
                        .SetDataType(x => x.Boolean());
                    cfg.AddField("Jak byste krátce představili Vaši společnost?", x => x.AdditionalQuestions)
                        .SetDataType(x => x.MultilineText());

                });
            }
        }

        public class CandidateRequestValidator : AbstractValidator<CandidateRequest>
        {
            public CandidateRequestValidator()
            {
                RuleFor(x => x.AdditionalQuestions)
                    .MaximumLength(WebDataConstants.MaximumShortRteLength)
                    .WithName(_ => this.Localize("Doplňující otázka", "Additional question"));
            }
        }
    }
}