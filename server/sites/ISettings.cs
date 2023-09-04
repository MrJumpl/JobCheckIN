using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Umbraco.Core.Models;

namespace Mlok.Web.Sites.JobChIN
{
    public interface ISettings
    {
        string AditionalEducationDescription { get; }
        string CareerPortfolioDescription { get; }
        string CareerVisionDescription { get; }
        string CompanyAfterLogin { get; }
        string CompanyGDPR { get; }
        string CompanyNoLogoLink { get; }
        string CompanyRegistrationSuccessful { get; }
        string PresentationDescription { get; }
        string RouteMyOffers { get; }
        string RouteNotifications { get; }
        string RouteCompanyAboutUs { get; }
        string RouteCompanyStudentSearch { get; }
        string RouteStudentCV { get; }
        string RouteStudentCVBasicInfo { get; }
        string StudentAfterLogin { get; }
        int StudentAgreedTo { get; }
        string StudentGDPR { get; }
        IPublishedContent StudentMemberZone { get; }
        string StudentRegistrationText { get; }
        string StudentSkillsDescription { get; }
        IPublishedContent StudentCvDocxTemplate { get; }
        string Visibility { get; }
        int WorkPositionMaxDuration { get; }
        int WorkPositionsPerPage { get; }

        string RenderWorkPosition(WorkPositionDetailDto workPosition);
        string ShowInterestText(int completeness, string companyName);
    }
}