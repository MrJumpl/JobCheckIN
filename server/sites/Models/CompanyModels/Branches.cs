using Mlok.Modules.WebData;
using System.Collections.Generic;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Models.CompanyModels
{
    [ModelEditor(typeof(CompanyBranchesWebDataFormatter))]
    public class CompanyBranches : IAnonymizable
    {
        public IEnumerable<Branch> Branches { get; set; }

        public void AnonymizeData()
        {
            Branches = Enumerable.Empty<Branch>();
        }


        public class CompanyBranchesWebDataFormatter : AbstractModelEditor<CompanyBranches, JobChINModule>
        {
            public CompanyBranchesWebDataFormatter()
            {
                SetupSimpleModelEditor(cfg =>
                {
                    cfg.AddField("Pobočky", x => x.Branches);
                });
            }
        }
    }
}