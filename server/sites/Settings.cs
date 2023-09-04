using Mlok.Core.Utils;
using Mlok.Umbraco;
using Mlok.Umbraco.PropertyEditors.Models;
using Mlok.Web.Sites.JobChIN.Models;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Our.Umbraco.Vorto.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Mlok.Web.Sites.JobChIN
{
    public class Settings : ISettings
    {
        private readonly SiteHelper site;
        private readonly IPublishedContent settingsNode;

        public Settings(SiteHelper site, IPublishedContent settingsNode)
        {
            this.site = site;
            this.settingsNode = settingsNode;
        }

        public string CompanyGDPR => GetUrl("companyGDPR");
        public string CompanyRegistrationSuccessful => RenderSnippet("companyRegistrationSuccessful");
        public string CompanyAfterLogin => RenderSnippet("companyAfterLogin");
        public string CompanyNoLogoLink => settingsNode.GetPropertyValue<FocalPointImage>("companyNoLogo")?.Url;

        public string StudentGDPR => GetUrl("studentGDPR");
        public int StudentAgreedTo => settingsNode.GetPropertyValue("studentAgreedTo", 12);
        public string StudentRegistrationText => RenderSnippet("studentRegistrationText");
        public string StudentAfterLogin => GetUrl("studentAfterLogin");
        public IPublishedContent StudentMemberZone => settingsNode.GetPropertyValue<IPublishedContent>("studentMemberZone");
        public string StudentSkillsDescription => RenderSnippet("skillsDescription");
        public IPublishedContent StudentCvDocxTemplate => settingsNode.GetVortoValue<IPublishedContent>("studentCvDocxTemplate");
        public string Visibility => RenderSnippet("visibility");

        public int WorkPositionMaxDuration => settingsNode.GetPropertyValue("workPositionMaxDuration", 3);
        public int WorkPositionsPerPage => 10;

        public string CareerVisionDescription => settingsNode.GetVortoValue<string>("careerVisionDescription");
        public string PresentationDescription => settingsNode.GetVortoValue<string>("presentationDescription");
        public string CareerPortfolioDescription => settingsNode.GetVortoValue<string>("careerPortfolioDescription");
        public string AditionalEducationDescription => settingsNode.GetVortoValue<string>("aditionalEducationDescription");

        public string RouteStudentCV => RouteProfile + "cv";
        public string RouteCompanyAboutUs => RouteProfile + this.Localize("o-firme", "about-company");
        public string RouteCompanyStudentSearch => RouteProfile + this.Localize("vyhledavani-studentu", "student-search"); 
        public string RouteStudentCVBasicInfo => $"{RouteStudentCV}/{this.Localize("zakladni-informace", "basic-informations")}";
        public string RouteMyOffers => RouteProfile + this.Localize("moje-nabidky", "my-offers");
        public string RouteNotifications => RouteProfile + this.Localize("notifikace", "notification");

        public string RenderWorkPosition(WorkPositionDetailDto workPosition) => MvcUtils.RenderView(site.PartialViews().JobChIN_WorkPositionDetailView, workPosition, currentPage: settingsNode);
        public string ShowInterestText(int completeness, string companyName)
        {
            var models = settingsNode.GetPropertyValue<IEnumerable<IPublishedContent>>("interestTexts")
                .OrderBy(x => x.GetPropertyValue<int>("completeness"));
            IPublishedContent model = null;
            foreach (var item in models)
            {
                if (item.GetPropertyValue<int>("completeness") >= completeness)
                {
                    model = item;
                    break;
                }
            }
            if (model == null)
                return string.Empty;

            var templateValues = new Dictionary<string, string>();
            templateValues.Add("completeness", completeness.ToString());
            templateValues.Add("companyName", companyName);

            return ReplaceTemplateValues(model.GetVortoRichtextValueString("text"), templateValues);
        }

        private string RenderSnippet(string propertyAlias)
        {
            var content = settingsNode.GetPropertyValue<IPublishedContent>(propertyAlias);
            var grid = content?.GetVortoValue("content");
            if (grid != null)
                return MvcUtils.RenderView(site.PartialViews().GridLayoutView, grid, null, content);
            return string.Empty;
        }

        private string GetUrl(string propertyAlias)
        {
            var content = settingsNode.GetPropertyValue<IPublishedContent>(propertyAlias);
            return content.Url();
        }

        private string RouteProfile => $"/{this.Localize("profil", "profile")}/";

        private string ReplaceTemplateValues(string body, Dictionary<string, string> templateValues)
        {
            if (templateValues != null)
                foreach (var templateValue in templateValues)
                    body = body.Replace($"{{{{{templateValue.Key}}}}}", templateValue.Value ?? string.Empty);

            return body;
        }
    }
}