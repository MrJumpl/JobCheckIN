using AutoMapper;
using Mlok.Core.Data;
using Mlok.Core.Models;
using Mlok.Core.Models.ApiExceptions;
using Mlok.Core.Services;
using Mlok.Core.Services.OC;
using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Modules.WebData.Members;
using Mlok.Umbraco;
using Mlok.Web.Sites.JobChIN.Constants;
using Mlok.Web.Sites.JobChIN.Controllers;
using Mlok.Web.Sites.JobChIN.Members;
using Mlok.Web.Sites.JobChIN.Models;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Mlok.Web.Sites.JobChIN.Models.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.PublishedCache;

namespace Mlok.Web.Sites.JobChIN.Services
{
	public class CompanyService : UserService<CompanyUser>
	{
		private readonly ISettings settings;
		private readonly OCService ocService;
		private readonly CompanyFileService fileService;
		private readonly AresService aresService;
		private readonly WorkPositionService workPositionService;
		private readonly DbScopeProvider scopeProvider;

		private readonly CompanyAreaOfInterestController companyAreaOfInterestController;
		private readonly CompanyFacultyController companyFacultyController;
		private readonly CompanyLanguageSkillPreferredController companyLanguageSkillPreferredController;
		private readonly CompanyCompanyTypeController companyCompanyTypeController;
		private readonly CompanyTypeController companyTypeController;
		private readonly CompanyBranchController companyBranchController;
		private readonly CompanyUserController companyUserController;


		public CompanyController CompanyController { get; }

		protected override bool IsStudent => false;
		protected override string ClassIdentifier => "Company";

		public CompanyService(DbScopeProvider scopeProvider, JobChINMembersPlugin membersPlugin, ISettings settings, OCService ocService, CompanyFileService fileService, AresService aresService, WorkPositionService workPositionService)
			: base(membersPlugin)
		{
			this.scopeProvider = scopeProvider;
			this.settings = settings;
			this.ocService = ocService;
			this.fileService = fileService;
			this.aresService = aresService;
			this.workPositionService = workPositionService;
			companyAreaOfInterestController = new CompanyAreaOfInterestController(scopeProvider);
			companyFacultyController = new CompanyFacultyController(scopeProvider);
			companyLanguageSkillPreferredController = new CompanyLanguageSkillPreferredController(scopeProvider);
			companyCompanyTypeController = new CompanyCompanyTypeController(scopeProvider);
			companyTypeController = new CompanyTypeController(scopeProvider);
			companyBranchController = new CompanyBranchController(scopeProvider);
			companyUserController = new CompanyUserController(scopeProvider);

			CompanyController = new CompanyController(scopeProvider);
		}

        /// <summary>
        /// Validate if the ICO is not registred in system and if the ICO exists.
        /// </summary>
        /// <param name="ico">ICO to check.</param>
		public async Task<Result<CompanyGeneralInfoDto>> ValidateCompanyIco(string ico)
		{
			var result = ValidateIcoUniqueness<CompanyGeneralInfoDto>(ico);
			if (result != null)
				return result;
			result = await aresService.GetCompanyInfo(ico);
			return result;
		}

        /// <summary>
        /// Returns the invoice for the purcahsed company type. Check if the oc request id belongs to company.
        /// </summary>
        /// <param name="requestId">OC request id.</param>
        public WebDataDownloadResult DownloadInvoice(int requestId)
		{
			var company = GetCurrent();
			if (!companyCompanyTypeController.ValidateRequestId(company.CompanyId, requestId))
				return new WebDataDownloadResult("Not found", WebDataActionResultType.Error);

			var invoice = ocService.GetInvoice(requestId);
			return new WebDataDownloadResult(invoice);
		}

        /// <summary>
        /// Register the company. 
        /// </summary>
        /// <param name="model">Company model to create.</param>
        public async Task<Result<string>> Register(CompanyCreateDto model)
		{
			var result = ValidateIcoUniqueness<string>(model.GeneralInfo.Ico);
			if (result != null)
				return result;
			result = await aresService.IcoExists(model.GeneralInfo.Ico);
			if (!result.IsValid())
				return result;

			using (var scope = scopeProvider.CreateScope())
			{
				Company company = Mapper.Map<Company>(model);
				fileService.CreateFolder(company);
				CompanyController.Update(company);
				CompanyUser user = Mapper.Map<CompanyUser>(model);
				MemberPublishedContent member = membersPlugin.Register(model.SignUpModel, user.FullName);
				user.Member = member;
				user.CompanyId = company.CompanyId;
				user.Role = Role.CompanyAdmin;
				user.NotificationSettings = new NotificationSettings()
				{
					NotificationFrequency = NotificationFrequency.Immediately,
				};
				companyUserController.Register(user);
				company.Users = user.AsEnumerableOfOne();
				result.Model = settings.CompanyRegistrationSuccessful;
				scope.Complete();
			}
			return result;
		}

        /// <summary>
        /// Create the OC request and return the url where to redirect user.
        /// </summary>
        /// <param name="model">Model that represent which package the company wants to buy.</param>
        public Result<string> BuyCompanyType(BuyCompanyTypeDto model)
		{
			var companyType = companyTypeController.GetById(model.CompanyTypeId);
			var company = GetCurrentCompany();
			var orderRequest = new OrderRequestModel()
			{
				Packages = new List<PackageRequestModel>() { new PackageRequestModel()
				{
					Id = company.IsProfitSector() ? companyType.PackageProfit.Value : companyType.PriceNonProfit.Value,
					Count = 1
				} },
				Language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName,
			};
			orderRequest = Mapper.Map(GetCurrent(), orderRequest);
			orderRequest = Mapper.Map(company, orderRequest);
			var ocResult = ocService.PutOrderRequest(orderRequest, "99", membersPlugin.Module.Id);

			var companyCompanyType = Mapper.Map<CompanyCompanyType>(companyType);
			companyCompanyType.ActiveFrom = model.ActiveFrom;
			companyCompanyType.ActiveTo= model.ActiveFrom.AddMonths(companyType.Validity).AddDays(-1);
			companyCompanyType.OCRequestId = ocResult.RequestId;

			companyCompanyTypeController.Add(company.CompanyId, companyCompanyType);

			return new Result<string>(ocResult.OCUrl);
		}

        /// <summary>
        /// Update status of the order requests. The canceled ids will be deleted.
        /// </summary>
        /// <param name="paidRequestIds">Requests id that are paid.</param>
        /// <param name="cancelledRequestIds">Requests id that are canceled.</param>
		public void UpdateOrdersStatus(IEnumerable<int> paidRequestIds, IEnumerable<int> cancelledRequestIds)
		{
			PayOrders(paidRequestIds);
			CancelOrders(cancelledRequestIds);
		}

        /// <summary>
        /// Return the url where to redirect after return from the OC.
        /// </summary>
        /// <param name="requestId">OC request id.</param>
        /// <param name="orderId">The order id in OC.</param>
        /// <param name="isPaid">Is the order paid.</param>
		public OCReturnRedirectMeta GetOCReturnRedirect(int requestId, int orderId, bool isPaid)
		{
			return new OCReturnRedirectMeta()
			{
				Url = $"{membersPlugin.AngularAppNode.UrlAbsolute()}?{SiteConstants.OCReturnParam}=true&{SiteConstants.OCPaidParam}={isPaid}",
			};
		}

        /// <summary>
        /// Invites new user by admin. Sends an email notification to the new user.
        /// </summary>
        /// <param name="ids">The company ids.</param>
        /// <param name="config">Config that contains new user email.</param>
		public void InviteUser(IEnumerable<int> ids, SendUserInvitaion config)
		{
			foreach (var id in ids)
				membersPlugin.SendNewUserInvitaion(config, null, id, false);
		}

        /// <summary>
        /// Register the nwe user to the given company. 
        /// </summary>
        /// <param name="model">New user model.</param>
        public Result<string> CreateNewUser(NewCompanyUserDto model)
		{
			InviteCompanyUserModel tokenModel = null;
			var tokenValid = membersPlugin.CheckNewUserToken(model.Token, out tokenModel);
			if (tokenValid == TokenValidationState.Invalid || tokenValid == TokenValidationState.None)
				return new Result<string>(new TokenExpiredException(string.Empty));
			bool isSameEmail = model.SignUpModel.Email == tokenModel.Email;

			using (var scope = scopeProvider.CreateScope())
			{
                CompanyUser user = new CompanyUser()
                {
                    CompanyId = tokenModel.CompanyId,
                    Role = tokenModel.Role,
                    ContactPerson = model.ContactPerson,
                    NotificationSettings = new NotificationSettings()
                    {
                        NotificationFrequency = NotificationFrequency.Immediately,
					},
				};
				MemberPublishedContent newMember = membersPlugin.Register(model.SignUpModel, user.FullName, isSameEmail);
				user.Member = newMember;
				companyUserController.Register(user);

				// delete member after company member is updated, otherwise the company will be anonymized
				if (tokenModel.MemberId.HasValue)
					membersPlugin.DeleteMember(tokenModel.MemberId.Value);
				if (isSameEmail)
					membersPlugin.LoginMember(newMember);

				scope.Complete();
			}

			return new Result<string>(isSameEmail ? membersPlugin.AngularAppNode.Url() : string.Empty);
		}

        /// <summary>
        /// Deletes the user by its membeId.
        /// </summary>
        public Result DeleteUser(int memberId)
		{
			var currentUser = GetCurrent();
			if (memberId == currentUser.MemberId)
				return new Result(new BadRequestException("Can not delete same user"));

			var user = companyUserController.GetByMemberId(memberId);
			if (user == null || user.CompanyId != currentUser.CompanyId)
				return new Result<CompanyUserViewDto>(new NotFoundException($"User with id '{memberId}' has not been found."));

			using (var scope = scopeProvider.CreateScope())
			{
				membersPlugin.DeleteMember(memberId);
				scope.Complete();
			}
			return new Result();
		}

        /// <summary>
        /// Changes the role of given user.
        /// </summary>
        /// <param name="model">Model that identifies the user and role.</param>
        public Result<CompanyUserViewDto> ChangeRole(ChangeRoleDto model)
		{
			var currentUser = GetCurrent();
			if (model.MemberId == currentUser.MemberId)
				return new Result<CompanyUserViewDto>(new BadRequestException("Can not change your own role."));
			
			var user = companyUserController.GetByMemberId(model.MemberId);
			if (user == null ||user.CompanyId != currentUser.CompanyId)
				return new Result<CompanyUserViewDto>(new NotFoundException($"User with id '{model.MemberId}' has not been found."));

			using (var scope = scopeProvider.CreateScope())
			{
				user.Role = model.Role;
				companyUserController.UpdateRole(user);
				scope.Complete();
			}
			return new Result<CompanyUserViewDto>(Mapper.Map<CompanyUserViewDto>(user));
		}

        /// <summary>
        /// Updates the company current state. Only properties that are not null, will be updated.
        /// </summary>
        /// <param name="model">Company model to udpate.</param>
        public Result<CompanyUpdateDto> Update(CompanyUpdateDto model)
		{
			Company company = Mapper.Map<Company>(model);
			var oldCompany = GetCurrentCompany();
			company.CompanyId = oldCompany.CompanyId;
			company.MediaFolderId = oldCompany.MediaFolderId;
			if (company.GeneralInfo != null)
			{
				company.GeneralInfo.Ico = oldCompany.GeneralInfo.Ico;
				company.GeneralInfo.Sector = oldCompany.GeneralInfo.Sector;
			}
			company = Update(company, oldCompany);
			return new Result<CompanyUpdateDto>(Mapper.Map<CompanyUpdateDto>(company));
		}

        /// <summary>
        /// Updates company model also with joined data. Only properties that are not null, will be updated.
        /// </summary>
        /// <param name="model">New state to update.</param>
        /// <param name="oldModel">Current state of the company in database. If it is null, then it is get from database.</param>
		public Company Update(Company model, Company oldModel = null)
		{
			if (oldModel == null)
				oldModel = CompanyController.GetById(model.CompanyId);

			CompanyController.Update(model);
			UpdateJoinedData(model, oldModel);

			return model;
		}

        /// <summary>
        /// Updates the company user current state. Only properties that are not null, will be updated.
        /// </summary>
        /// <param name="model">Company user model to update.</param>
        public Result<CompanyUserUpdateDto> UpdateUser(CompanyUserUpdateDto model)
		{
			var user = GetCurrent();
			CompanyUser companyUser = Mapper.Map<CompanyUser>(model);
            companyUser.Member = user.Member;
            
			if (companyUser.ContactPerson != null)
				membersPlugin.UpdateMember(user.Member.Id, companyUser.FullName);
			companyUser = companyUserController.Update(companyUser);
			return new Result<CompanyUserUpdateDto>(Mapper.Map<CompanyUserUpdateDto>(companyUser));
		}

        /// <summary>
        /// Ids of companies that are confirmed by admin.
        /// </summary>
        /// <param name="ids">Ids of companies.</param>
		public WebDataActionResult ConfirmCompanies(IEnumerable<int> ids)
		{
			int numberOfConfirmed = CompanyController.ConfirmCompany(ids);
			return new WebDataActionResult($"Počet potvrzených zaměstnavatelů: {numberOfConfirmed}");
		}

        /// <summary>
        /// Delete the comapny that is not confirmed.
        /// </summary>
        /// <param name="ids">Ids of companies.</param>
		public WebDataActionResult DeleteCompanies(IEnumerable<int> ids)
		{
			foreach (var id in ids)
			{
				var users = companyUserController.GetMemberIdsByCompanyId(id);
				foreach (var userId in users)
					membersPlugin.DeleteMember(userId);
			}

			CompanyController.Delete(ids);
			return new WebDataActionResult($"Smazání hotovo");
		}

        /// <summary>
        /// Returns the picker of the company branches. If the company id is null, then returns empty list.
        /// </summary>
        /// <param name="companyId">The id of the company.</param>
		public IEnumerable<EnumerablePickerValue<int?, string>> GetCompanyBranchPicker(int? companyId)
		{
			if (!companyId.HasValue)
				return Enumerable.Empty<EnumerablePickerValue<int?, string>>();
			return companyBranchController.GetByCompanyId(companyId.Value)
				.Select(x => EnumerablePickerValue.From<int?, string>(x.BranchId, x.GetTitle()));
		}

        /// <summary>
        /// Returns the branch for the company. If the branch does not belong to the company then it returns null.
        /// </summary>
		public Branch GetBranch(int companyId, int branchId) => companyBranchController.GetByCompanyId(companyId, branchId);

        /// <summary>
        /// Returns the companies that match the filter.
        /// </summary>
        /// <param name="filter">Model to be filtered by.</param>
        public Result<IEnumerable<StudentCompanyListViewDto>> GetActiveCompanies(CompanyFilterDto filterDto)
		{
			IPaginationInfo paginationInfo = null;
			var filter = Mapper.Map<CompanyFilter>(filterDto);
			filter.IsConfirmed = true;
			filter.Active = true;
			filter.Archived = false;
			filter.IncludeWorkPositionStats = true;
			filter.IncludeLogo = true;

			return new Result<IEnumerable<StudentCompanyListViewDto>>(CompanyController.GetPaged(1, int.MaxValue, filter, out paginationInfo)
				.Select(Mapper.Map<StudentCompanyListViewDto>)
				.OrderBy(x => x.CompanyName));
		}

		public Company GetCurrentCompany()
		{
			var user = GetCurrent();
			return CompanyController.GetById(user.CompanyId);
		}

		protected override CompanyUser GetByMemberId(int memberId) => companyUserController.GetByMemberId(memberId);

		public override void DeleteUserByMemberId(int memberId)
		{
			var user = companyUserController.GetByMemberId(memberId);
			companyUserController.Delete(memberId);

			if (!companyUserController.GetMemberIdsByCompanyId(user.CompanyId)?.Any() ?? true)
			{
				var company = CompanyController.GetById(user.CompanyId);
				company.AnonymizeData();
				CompanyController.CompositeUpdate(company);
				fileService.DeleteFolder(company);
				workPositionService.AnonymizeWorkPosition(company.CompanyId);
			}
		}

		protected override IEnumerable<int> GetMemberIds(int id) => companyUserController.GetMemberIdsByCompanyId(id);

		void PayOrders(IEnumerable<int> paidRequestIds)
		{
			var childs = companyCompanyTypeController.GetByRequestIds(paidRequestIds);
			foreach (var child in childs)
				companyCompanyTypeController.Pay(child);
			throw new NotImplementedException();
		}

		void CancelOrders(IEnumerable<int> cancelledRequestIds)
		{
			var childs = companyCompanyTypeController.GetByRequestIds(cancelledRequestIds);
			foreach (var child in childs)
				companyCompanyTypeController.Cancel(child);
			throw new NotImplementedException();
		}

		Result<T> ValidateIcoUniqueness<T>(string ico)
		{
			IPaginationInfo paginationInfo;
			CompanyController.GetPaged(1, 1, new CompanyFilter() { IsConfirmed = true, Ico = ico }, out paginationInfo);
			if (paginationInfo.ResultsCount > 0)
				return new Result<T>(new ConflictException("Company with ICO '{ico}' already exists"));
			return null;
		}

        /// <summary>
        /// Updates joined data. Only properties that are not null, will be updated.
        /// </summary>
        void UpdateJoinedData(Company model, Company oldModel)
		{
			if (model.Candidates != null)
			{
				companyAreaOfInterestController.Join(model);
				companyFacultyController.Join(model);
				companyLanguageSkillPreferredController.Join(model);
			}
			if (model.CompanyTypes != null)
				model.CompanyTypes = companyCompanyTypeController.Update(model.CompanyId, model.CompanyTypes);

			if (model.Presentation != null)
			{
				fileService.UpdateFiles(FileCategory.Logo | FileCategory.BackgroundImage, model, oldModel);
			}
			if (model.Branches != null)
			{
				model.Branches.Branches = companyBranchController.Update(model.CompanyId, model.Branches.Branches);
			}
            if (model.Users != null)
            {
                foreach (var user in oldModel.Users.Where(x => model.Users.All(y => y.MemberId != x.MemberId)))
                    membersPlugin.DeleteMember(user.MemberId);

                var newUsers = new List<CompanyUser>();
                foreach (var user in model.Users)
                {
                    newUsers.Add(companyUserController.CompositeUpdate(user));
                    membersPlugin.UpdateMember(user.MemberId, user.FullName);
                }
                model.Users = newUsers;
            }
		}
	}
}