using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Web.Sites.JobChIN.Models.WorkPositionModels;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    [Validator(typeof(WorkPositionCreateDtoValidator))]
    public class WorkPositionCreateDto
    {
        public BasicInfo BasicInfo { get; set; }
        public Detail Detail { get; set; }
        public Candidates Candidates { get; set; }
        public CandidateRequest CandidateRequest { get; set; }


        public class WorkPositionCreateDtoValidator : AbstractValidator<WorkPositionCreateDto>
        {
            public WorkPositionCreateDtoValidator()
            {
                RuleFor(x => x.BasicInfo)
                    .NotNull();

                RuleFor(x => x.Detail)
                    .NotNull();

                RuleFor(x => x.Candidates)
                    .NotNull();

                RuleFor(x => x.CandidateRequest)
                    .NotNull();
            }
        }
    }
}