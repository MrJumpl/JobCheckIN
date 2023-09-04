using Mlok.Modules.WebData;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Models.Filters
{
    [ModelEditor(typeof(HardSkillFilterWebDataFormatter))]
    public class HardSkillFilter
    {
        public string SkillName { get; set; }
        public int? AreaOfInterestId { get; set; }
        public string MuPartId { get; set; }

        public class HardSkillFilterWebDataFormatter : AbstractModelEditor<HardSkillFilter, JobChINModule>
        {
            public HardSkillFilterWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Název skillu", x => x.SkillName);
                    cfg.AddField("Oblast zájmu", x => x.AreaOfInterestId)
                        .SetDataType(x => x.SingleValuePicker(() => Module.AreaOfInterestController.GetAll().Select(aoi => new EnumerablePickerValue<int?, string>(aoi.AreaOfInterestId, aoi.Name))));
                    cfg.AddField("Fakulta/Katedra", x => x.MuPartId)
                        .SetDataType(x => x.DepartmentPicker());
                });
            }
        }
    }
}