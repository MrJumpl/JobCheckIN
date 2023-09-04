using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mlok.Web.Sites.JobChIN.Models
{
    [Validator(typeof(CompanyValidator))]
    [ModelEditor(typeof(CompanyWebDataFormatter))]
    public class Company : IUserMedia, IAnonymizable
    {
        public int CompanyId { get; set; }
        public bool Confirmed { get; set; }
        [JsonIgnore]
        public int MediaFolderId { get; set; }
        public IEnumerable<CompanyCompanyType> CompanyTypes { get; set; }
        public DateTime Created { get; set; }
        public GeneralInfo GeneralInfo { get; set; }
        public Candidates Candidates { get; set; }
        public Presentation Presentation { get; set; }
        public CompanyBranches Branches { get; set; }
        public IEnumerable<CompanyUser> Users { get; set; }

        public bool Archived { get; set; }
        public int? WorkPositionCount { get; set; }

        public bool IsProfitSector() => GeneralInfo.Sector == Sector.PrivateSector;

        public void AnonymizeData()
        {
            foreach (var item in GetAnonymizable())
                item.AnonymizeData();
        }

        IEnumerable<IAnonymizable> GetAnonymizable()
        {
            return new IAnonymizable[] { GeneralInfo, Candidates, Presentation, Branches };
        }


        public class CompanyWebDataFormatter : AbstractModelEditor<Company, JobChINModule>
        {
            public CompanyWebDataFormatter()
            {
                Setup(cfg =>
                {
                    cfg.SetName("Název společnosti", x => x.GeneralInfo.CompanyName);
                });

                SetupOverview(listviewCfg => { });

                SetupModelEditor(detailCfg =>
                {
                    detailCfg.AddTab("Typ účtu", tabConfig =>
                    {
                        tabConfig.AddField("Typ účtu", x => x.CompanyTypes);
                    });
                    detailCfg.AddTab("Základní informace", tabConfig =>
                    {
                        tabConfig.AddField(x => x.GeneralInfo);
                    });
                    detailCfg.AddTab("Uživatelé", tabConfig =>
                    {
                        tabConfig.AddField(x => x.Users);
                    });
                    detailCfg.AddTab("Uchazeči", tabConfig =>
                    {
                        tabConfig.AddField(x => x.Candidates);
                    });
                    detailCfg.AddTab("Prezentace", tabConfig =>
                    {
                        tabConfig.AddField(x => x.Presentation);
                    });
                    detailCfg.AddTab("Pobočky", tabConfig =>
                    {
                        tabConfig.AddField(x => x.Branches);
                    });
                });
            }
        }

        public class CompanyValidator : AbstractValidator<Company>
        {
            public CompanyValidator()
            {
                RuleFor(x => x.CompanyTypes)
                    .Must(companyTypes => {
                        if (companyTypes == null || companyTypes.Count() <= 1)
                            return true;
                        var sorted = companyTypes.OrderBy(x => x.ActiveFrom).ThenBy(x => x.ActiveTo);
                        var pivot = sorted.First();
                        foreach (var next in sorted.Skip(1))
                        {
                            if (pivot.ActiveTo >= next.ActiveFrom)
                                return false;
                            if (pivot.ActiveTo < next.ActiveTo)
                                pivot = next;
                        }
                        return true;
                    })
                    .WithMessage(x => this.Localize("Typy účtů se nesmí překrývat.", "Company types must not overlap."));

                RuleFor(x => x.Users)
                    .Must(x => x == null || x.Any())
                    .WithMessage(x => this.Localize("Nelze smazat všechny uživatele firmy.", "Cannot delete all users of a company."));
                RuleFor(x => x.Users)
                    .Must(x => x == null || x.Any(y => y.Role == Role.CompanyAdmin))
                    .WithMessage(x => this.Localize("Alespoň jeden uživatel musí být správcem firmy.", "At least one user must be company admin."));
            }
        }
    }
}