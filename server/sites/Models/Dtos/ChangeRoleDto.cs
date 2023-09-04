using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    [Validator(typeof(ChangeRoleDtoValidator))]
    public class ChangeRoleDto
    {
        public int MemberId { get; set; }
        public Role Role { get; set; }

        public class ChangeRoleDtoValidator : AbstractValidator<ChangeRoleDto>
        {
            public ChangeRoleDtoValidator()
            {
                RuleFor(x => x.Role)
                    .IsInEnum();
            }
        }
    }
}