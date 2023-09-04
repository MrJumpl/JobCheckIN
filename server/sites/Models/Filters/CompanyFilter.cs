using Mlok.Modules.WebData;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Filters
{
    [ModelEditor(typeof(CompanyFilterWebDataFormatter))]
    public class CompanyFilter
    {
        public string CompanyName { get; set; }
        public IEnumerable<int> AreaOfInterests { get; set; }
        public string Faculty { get; set; }
        public bool OnlyHasWorkPosition { get; set; }
        public Sector? Sector { get; set; }
        public string Ico { get; set; }

        public bool? IsConfirmed { get; set; }
        public bool Active { get; set; }
        public bool Archived { get; set; }
        public bool IncludeWorkPositionStats { get; set; }
        public bool IncludeLogo { get; set; }

        public class CompanyFilterWebDataFormatter : AbstractModelEditor<CompanyFilter, JobChINModule>
        {
            public CompanyFilterWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Název zaměstnavatele", x => x.CompanyName);
                    cfg.AddField("Oblast zájmu", x => x.AreaOfInterests)
                        .SetDataType(x => x.MultipleValuePicker(() => Module.AreaOfInterestController.GetPicker()));
                    cfg.AddField("Fakulta", x => x.Faculty)
                        .SetDataType(x => x.SingleValuePicker(() => Module.FacultyController.GetPicker()));
                    cfg.AddField("Sektor", x => x.Sector)
                        .SetDataType(x => x.SingleValuePicker(() => Module.SectorPicker));
                    cfg.AddField("Pouze firmy s inzeráty", x => x.OnlyHasWorkPosition)
                        .SetDataType(x => x.Boolean());
                });
            }
        }
    }
}