using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Constants;
using System;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Models.StudentModels
{
    [Validator(typeof(OtherStudyValidator))]
    [ModelEditor(typeof(OtherStudyWebDataFormatter))]
    public class OtherStudy : IStudy
    {
        public int OtherUniversityStudyId { get; set; }
        public string University { get; set; }
        public string Faculty { get; set; }
        public string Specialization { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }
        public DateTime From { get; set; } = DateTime.Now;
        public DateTime? To { get; set; }
        public int? LanguageId { get; set; }


        public class OtherStudyWebDataFormatter : AbstractModelEditor<OtherStudy, JobChINModule>
        {
            public OtherStudyWebDataFormatter()
            {
                Setup(cfg => {
                    cfg.SetId(x => x.OtherUniversityStudyId);
                    cfg.SetName("Název", x => x.GetTitle());
                });

                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Škola", x => x.University);
                    cfg.AddField("Fakulta", x => x.Faculty);
                    cfg.AddField("Obor", x => x.Specialization);
                    cfg.AddField("Místo", x => x.City);
                    cfg.AddField("Stát", x => x.CountryId)
                        .SerializeAs(x => x == default(int) ? null : (int?)x, x => x ?? default(int))
                        .SetDataType(x => x.SingleValuePicker(() => Module.CountryController.GetPicker()));
                    cfg.AddField("Od", x => x.From)
                        .SetDataType(x => x.DateTime(DateTimeFieldEnum.Date));
                    cfg.AddField("Do", x => x.To)
                        .SetDataType(x => x.DateTime(DateTimeFieldEnum.Date));
                    cfg.AddField("Jazyk", x => x.LanguageId)
                        .SetDataType(x => x.SingleValuePicker(() => Module.LanguageController.GetPicker()));
                });
            }
        }

        public class OtherStudyValidator : AbstractValidator<OtherStudy>
        {
            public OtherStudyValidator()
            {
                RuleFor(x => x.University)
                   .NotEmpty()
                   .MaximumLength(WebDataConstants.MaximumOtherStudyLength)
                   .WithName(_ => this.Localize("Škola", "University"));
                RuleFor(x => x.Faculty)
                   .MaximumLength(WebDataConstants.MaximumOtherStudyLength)
                   .WithName(_ => this.Localize("Fakulta", "Faculty"));
                RuleFor(x => x.Specialization)
                   .NotEmpty()
                   .MaximumLength(WebDataConstants.MaximumOtherStudyLength)
                   .WithName(_ => this.Localize("Obor", "Specialization"));
                RuleFor(x => x.City)
                   .NotEmpty()
                   .MaximumLength(WebDataConstants.MaximumStudentCityLength)
                   .WithName(_ => this.Localize("Místo", "City"));
                RuleFor(x => x.CountryId)
                    .GreaterThan(default(int))
                    .WithName(_ => this.Localize("Stát", "Country"));
                RuleFor(x => x.From)
                   .GreaterThan(DateTime.MinValue)
                   .WithMessage(_ => this.Localize(
                       "Pole 'Studoval/a jsem tady do' není vyplněno správně",
                       "")) // TODO: translate
                   .LessThan(DateTime.Now)
                   .WithMessage(_ => this.Localize(
                       "Pole 'Studoval/a jsem tady do' musí být v minulosti",
                       "The field 'I studied here until' must be in the past"));
                RuleFor(x => x.To)
                    .GreaterThan(x => x.From)
                    .When(x => x.To.HasValue)
                    .WithMessage(_ => this.Localize(
                        "Pole 'Studoval/a jsem tady do' není vyplněno správně",
                        "")); // TODO: translate
            }
        }

        string GetTitle()
        {
            string result = University ?? "";
            if (!string.IsNullOrWhiteSpace(University) && !string.IsNullOrWhiteSpace(Specialization))
                result += " - ";
            result += Specialization;

            return result;
        }
    }
}