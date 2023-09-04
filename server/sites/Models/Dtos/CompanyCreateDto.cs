using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Modules.WebData.Members;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    [Validator(typeof(CompanyCreateDtoValidator))]
    public class CompanyCreateDto
    {
        public MemberDataModel SignUpModel { get; set; }
        public ContactPerson ContactPerson { get; set; }
        public GeneralInfo GeneralInfo { get; set; }

        public class CompanyCreateDtoValidator : AbstractValidator<CompanyCreateDto>
        {
            public CompanyCreateDtoValidator()
            {
                RuleFor(x => x.SignUpModel)
                    .NotNull();
                RuleFor(x => x.ContactPerson)
                    .NotNull();
                RuleFor(x => x.GeneralInfo)
                    .NotNull();
            }
        }
    }
}