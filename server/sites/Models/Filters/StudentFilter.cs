using Mlok.Modules.WebData;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Filters
{
    [ModelEditor(typeof(StudentFilterWebDataFormatter))]
    public class StudentFilter
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public int? Uco { get; set; }
        public IEnumerable<int> AreaOfInterests { get; set; }
        public IEnumerable<int> HardSkills { get; set; }
        public int? WorkPositionId { get; set; }
        public int? HiddenWorkPositionId { get; set; }
        public int? CompanyId { get; set; }
        public bool Active { get; set; } = true;

        public class StudentFilterWebDataFormatter : AbstractModelEditor<StudentFilter, JobChINModule>
        {
            public StudentFilterWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Jméno", x => x.Firstname);
                    cfg.AddField("Příjmení", x => x.Surname);
                    cfg.AddField("Uco", x => x.Uco)
                        .SetDataType(x => x.Number());
                    cfg.AddField("Oblast zájmu", x => x.AreaOfInterests)
                        .SetDataType(x => x.MultipleValuePicker(() => Module.AreaOfInterestController.GetPicker()));
                    cfg.AddField("Znalosti a dovednosti", x => x.HardSkills)
                        .SetDataType(x => x.MultipleValuePicker(() => Module.HardSkillController.GetPicker()));
                });
            }
        }
    }
}