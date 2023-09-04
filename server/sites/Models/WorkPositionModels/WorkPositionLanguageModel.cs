using FluentValidation.Attributes;
using Mlok.Modules.WebData;

namespace Mlok.Web.Sites.JobChIN.Models.WorkPositionModels
{
    [Validator(typeof(LanguageModelValidator))]
    [ModelEditor(typeof(WorkPositionLanguageWebDataFormatter))]
    public class WorkPositionLanguageModel : LanguageModel
    {
        public bool Optional { get; set; }


        public class WorkPositionLanguageWebDataFormatter : LanguageWebDataFormatter<WorkPositionLanguageModel>
        {
            public WorkPositionLanguageWebDataFormatter() : base()
            { }


            protected override void SimpleModelEditor(SimpleModelEditorConfig<WorkPositionLanguageModel> cfg)
            {
                base.SimpleModelEditor(cfg);

                cfg.AddField("Výhodou", x => x.Optional)
                    .SetDataType(x => x.Boolean());
            }

            protected override string GetName(WorkPositionLanguageModel model)
            {
                var result = base.GetName(model);
                result += " (";
                result += model.Optional ? "Výhodou" : "Povinný";
                result += ")";
                return result;
            }
        }
    }
}