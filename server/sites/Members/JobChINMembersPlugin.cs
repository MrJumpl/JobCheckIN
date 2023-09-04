using AutoMapper;
using Mlok.Core.Models;
using Mlok.Core.Services.ExternalLoginProviders;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Modules.WebData.Members;
using Mlok.Umbraco.Modules.Plugins;
using Mlok.Web.Sites.JobChIN.Constants;
using Mlok.Web.Sites.JobChIN.Models;
using Our.Umbraco.Vorto.Extensions;
using System;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.PublishedCache;

namespace Mlok.Web.Sites.JobChIN.Members
{
    [ModulePlugin("JobChIN_MemberSettings", HasWebData = true)]
    public class JobChINMembersPlugin : WebDataMembersPlugin
    {
        const string TOKEN_DATA_TYPE = "takeOverAccountToken";
        readonly JobChINModule jobchinModule;

        public JobChINMembersPlugin(CustomWebDataModule module, IPublishedContent pluginNode) : base(module, pluginNode)
        {
            jobchinModule = (JobChINModule)module;
        }

        public int GetCurrentUserUco() => _externalLoginService.GetProvider<MuniExternalLoginProvider>().CurrentUserUco;
        public string GetCurrentUserName() => _externalLoginService.GetProvider<MuniExternalLoginProvider>()?.CurrentUserGivenName;

        public override IPublishedContent AngularAppNode
        {
            get
            {
                var member = GetCurrentMember();
                if (member == null)
                    return base.AngularAppNode;
                if (IsStudent(member.Id))
                    return jobchinModule.Settings.StudentMemberZone;
                return base.AngularAppNode;
            }
        }

        public bool IsStudent(int memberId)
        {
            var stored = IsStoredStudent(memberId);
            if (stored.HasValue)
                return stored.Value;
            var student = jobchinModule.StudentService.GetCurrent(memberId);
            if (student != null)
                return true;
            var company = jobchinModule.CompanyService.GetCurrent(memberId);
            if (company != null)
                return false;
            throw new MemberNotExistsException(memberId);
        }

        public virtual Result<string> SendNewUserInvitaion(SendUserInvitaion config, MemberPublishedContent member, int companyId, bool bomberMailProtection = true)
        {
            var result = BomberMailProtection<string>(member, bomberMailProtection);
            if (result != null)
                return result;
            result = new Result<string>(config.Email);

            var token = Mapper.Map<InviteCompanyUserModel>(config);
            token.ValidUntil = DateTime.Now.AddDays(1);
            token.CompanyId = companyId;
            token.MemberId = config.DeleteMember ? member?.Id : null;
            _jsonStorage.Store(TOKEN_DATA_TYPE, token);

            var tokenParam = HttpUtility.UrlEncode(MachineKeyWrapper.Protect(token.Id.ToString()));
            Module.SendGridEmail(new GridEmailParams(PluginNode, "emailTakeOverAccountBody", "emailTakeOverAccountSubject", token.Email)
            {
                TemplateValues = new
                {
                    takeOverAccountLink = $"{PluginNode.GetVortoValue<IPublishedContent>("takeOverAccountPage").UrlAbsolute()}?{SiteConstants.TakeOverParam}={tokenParam}",
                    companyName = member.Name,
                    linkDate = DateUtils.FormatDate(token.ValidUntil.Value),
                }
            });
            return result;
        }

        public virtual TokenValidationState CheckNewUserToken(string token, out InviteCompanyUserModel model)
        {
            model = null;
            if (token != null)
            {
                bool tokenInvalid = false;
                try
                {
                    var tokenId = UrlUtils.DecryptUrlParam(token).TryConvertToInt(0);
                    model = _jsonStorage.GetObject<InviteCompanyUserModel>(tokenId);
                    tokenInvalid = model == null;
                }
                catch
                {
                    tokenInvalid = true;
                    model = null;
                }

                return tokenInvalid
                    ? TokenValidationState.Invalid
                    : TokenValidationState.Ok;
            }

            return TokenValidationState.None;
        }

        public bool? IsStoredStudent(int memberId)
        {
            return (bool?)Site.UmbracoContext.HttpContext.Items[$"JobChIN_MembersPlugin_IsStudent_{memberId}"];
        }

        public T GetStoredModel<T>(string classIdentifier)
            where T : IUser
        {
            return (T)Site.UmbracoContext.HttpContext.Items[$"JobChIN_MembersPlugin_{classIdentifier}"];
        }

        public void StoreCurrentModel<T>(T model, string classIdentifier, bool isStudent)
            where T : IUser
        {
            Site.UmbracoContext.HttpContext.Items[$"JobChIN_MembersPlugin_IsStudent_{model.Member.Id}"] = isStudent;
            Site.UmbracoContext.HttpContext.Items[$"JobChIN_MembersPlugin_{classIdentifier}"] = model;
        }

        protected override string GetDefaultExternalName(IExternalLoginProvider provider) => provider.CurrentUserName;
    }
}