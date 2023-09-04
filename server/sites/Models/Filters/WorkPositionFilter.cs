using Mlok.Modules.WebData;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.Filters
{
    [ModelEditor(typeof(WorkPositionFilterWebDataFormatter))]
    public class WorkPositionFilter
    {
        public IEnumerable<int> AreaOfInterests { get; set; }
        public IEnumerable<string> Locations { get; set; }
        public IEnumerable<int> ContractTypes { get; set; }
        public string Name { get; set; }
        public IEnumerable<Remote> Remotes { get; set; }
        public IEnumerable<int> Languages { get; set; }
        public OrderBy OrderBy { get; set; }

        public int? CompanyId { get; set; }
        public int? CompanyUserId { get; set; }
        public int? StudentId { get; set; }
        public bool? Active { get; set; }
        public bool? Published { get; set; }
        public bool? Hidden { get; set; }
        public bool IncludeHidden { get; set; }
        public bool IncludeStats { get; set; }
        public bool IncludeCompany { get; set; }
        public bool IncludeStudentDetails { get; set; }

        public class WorkPositionFilterWebDataFormatter : AbstractModelEditor<WorkPositionFilter, JobChINModule>
        {
            public WorkPositionFilterWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Firma", x => x.CompanyId)
                        .SetDataType(x => x.SingleValuePicker(() => Module.CompanyService.CompanyController.GetPicker(), enableSearch: true));
                });
            }
        }
    }
}