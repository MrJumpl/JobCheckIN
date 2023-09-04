using FluentValidation;
using Mlok.Modules.WebData;
using Mlok.Core.Utils;
using FluentValidation.Attributes;

namespace Mlok.Web.Sites.JobChIN.Models
{
    [Validator(typeof(WorkerCountRangeValidator))]
    [ModelEditor(typeof(WorkerCountRangeWebDataFormatter))]
    public class WorkerCountRange
    {
        public int WorkerCountRangeId { get; set; }
        public int? Low { get; set; }
        public int? High { get; set; }

        public string GetName()
        {
            if (Low.HasValue && High.HasValue)
                return $"{Low.Value} - {High.Value}";
            if (Low.HasValue)
                return $"{this.Localize("více než", "more than")} {Low.Value}";
            if (High.HasValue)
                return $"{this.Localize("méně než", "less than")} {High.Value}";
            return string.Empty;
        }

        public class WorkerCountRangeWebDataFormatter : AbstractModelEditor<WorkerCountRange, JobChINModule>
        {
            public WorkerCountRangeWebDataFormatter()
            {
                Setup(cfg =>
                {
                    cfg.SetName("Název", x => x.GetName(), namePropertyHide: true);
                });

                SetupOverview(listviewCfg => { });

                SetupModelEditor(detailCfg =>
                {
                    detailCfg.AddTab("Metadata", tabConfig =>
                    {
                        tabConfig.AddField("Od", x => x.Low)
                            .SetDataType(x => x.Number());
                        tabConfig.AddField("Do", x => x.High)
                            .SetDataType(x => x.Number());
                    });
                });
            }
        }

        public class WorkerCountRangeValidator : AbstractValidator<WorkerCountRange>
        {
            public WorkerCountRangeValidator()
            {
                RuleFor(x => x.Low)
                    .NotEmpty()
                    .When(workerCountRange => !workerCountRange.High.HasValue);

                RuleFor(x => x.High)
                    .NotEmpty()
                    .When(workerCountRange => !workerCountRange.Low.HasValue);

                RuleFor(x => x.Low)
                    .LessThan(workerCountRange => workerCountRange.High)
                    .When(workerCountRange => workerCountRange.High.HasValue);
            }
        }
    }
}