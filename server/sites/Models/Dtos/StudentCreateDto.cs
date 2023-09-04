using FluentValidation;
using FluentValidation.Attributes;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    [Validator(typeof(StudentCreateDtoValidator))]
    public class StudentCreateDto
    {
        public string ExternalLoginProvider { get; set; }
        public bool ProvideContact { get; set; }
        public bool GdprAgreement { get; set; }

        public class StudentCreateDtoValidator : AbstractValidator<StudentCreateDto>
        {
            public StudentCreateDtoValidator()
            {
                RuleFor(x => x.GdprAgreement)
                    .Equal(true);
            }
        }
    }
}