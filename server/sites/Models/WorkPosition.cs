using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using Mlok.Web.Sites.JobChIN.Models.WorkPositionModels;
using Candidates = Mlok.Web.Sites.JobChIN.Models.WorkPositionModels.Candidates;

namespace Mlok.Web.Sites.JobChIN.Models
{
    [Validator(typeof(WorkPositionValidator))]
    [ModelEditor(typeof(WorkPositionWebDataFormatter))]
    public class WorkPosition : IAnonymizable
    {
        public int WorkPositionId { get; set; }
        public int? CompanyId { get; set; }
        public DateTime Created { get; set; }

        public Visibility Visibility { get; set; }
        public BasicInfo BasicInfo { get; set; }
        public Detail Detail { get; set; }
        public Candidates Candidates { get; set; }
        public CandidateRequest CandidateRequest { get; set; }

        // automated data from database
        public string CompanyName { get; set; }
        public int? CompanyLogo { get; set; }
        public int ShownInterestCount { get; set; }
        public int ViewsCount { get; set; }
        public bool StudentFavorite { get; set; }
        public MatchCategory Match { get; set; }

        public bool HasAccess(CompanyUser companyUser)
        {
            if (companyUser.CompanyId != CompanyId.Value)
                return false;
            if (companyUser.Role != Role.CompanyAdmin && BasicInfo.Users.All(x => x != companyUser.MemberId))
                return false;
            return true;
        }

        public void AnonymizeData()
        {
            foreach (var item in GetAnonymizable())
                item.AnonymizeData();
        }

        IEnumerable<IAnonymizable> GetAnonymizable()
        {
            return new IAnonymizable[] { Visibility, BasicInfo, Detail, CandidateRequest };
        }


        public class WorkPositionWebDataFormatter : AbstractModelEditor<WorkPosition, JobChINModule>
        {
            public WorkPositionWebDataFormatter()
            {
                Setup(cfg =>
                {
                    cfg.SetName("Název", x => x.BasicInfo.Name, namePropertyHide: true);
                });

                SetupOverview(listviewCfg =>
                {
                    listviewCfg.AddField("Firma", x => string.IsNullOrWhiteSpace(x.CompanyName) ? "(deleted)" : x.CompanyName);
                    listviewCfg.AddField("Počet zájemců", x => x.ShownInterestCount);
                    listviewCfg.AddField("Počet zobrazení", x => x.ViewsCount);
                    listviewCfg.AddField("Datum vytvoření", x => x.Created);
                });

                SetupModelEditor(detailCfg =>
                {
                    detailCfg.AddTab("Základní informace", tabConfig =>
                    {
                        tabConfig.AddField("Firma", x => x.CompanyId)
                            .SetDataType(x => x.SingleValuePicker(() => Module.CompanyService.CompanyController.GetPicker(), enableSearch: true));
                        tabConfig.AddDynamicFieldGroup(x => x.CompanyId, rec => 
                        {
                            rec.BasicInfo.CurrentCompanyId = rec.CompanyId;
                            tabConfig.AddField(x => x.BasicInfo);
                        });
                    });
                    detailCfg.AddTab("Detail pracovní pozice", tabConfig =>
                    {
                        tabConfig.AddField(x => x.Detail);
                    });
                    detailCfg.AddTab("Uchazeči", tabConfig =>
                    {
                        tabConfig.AddField(x => x.Candidates);
                    });
                    detailCfg.AddTab("Doplňující otázka", tabConfig =>
                    {
                        tabConfig.AddField(x => x.CandidateRequest);
                    });
                });
            }
        }

        public class WorkPositionValidator : AbstractValidator<WorkPosition>
        {
            public WorkPositionValidator()
            {
                RuleFor(x => x.CompanyId)
                    .NotNull()
                    .WithName(_ => this.Localize("Zaměstnavatel", "Employer"));
            }
        }

    }
}