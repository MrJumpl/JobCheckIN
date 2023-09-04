using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Modules.WebData.Members;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    [Validator(typeof(NewCompanyUserDtoValidator))]
    public class NewCompanyUserDto
    {
        public string Token { get; set; }
        public MemberDataModel SignUpModel { get; set; }
        public ContactPerson ContactPerson { get; set; }

        public class NewCompanyUserDtoValidator : AbstractValidator<NewCompanyUserDto>
        {
            public NewCompanyUserDtoValidator()
            {
                RuleFor(x => x.SignUpModel)
                    .NotNull();
                RuleFor(x => x.ContactPerson)
                    .NotNull();
            }
        }
    }
}