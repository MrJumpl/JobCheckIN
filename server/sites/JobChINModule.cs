using AutoMapper;
using Mlok.Modules;
using Mlok.Modules.WebData;
using Mlok.Modules.WebData.Logging;
using Mlok.Modules.WebData.Members;
using Mlok.Umbraco;
using Mlok.Umbraco.Modules.Custom;
using Mlok.Web.Sites.JobChIN.Constants;
using Mlok.Web.Sites.JobChIN.Controllers;
using Mlok.Web.Sites.JobChIN.Models.Filters;
using Mlok.Web.Sites.JobChIN.Models;
using Mlok.Web.Sites.JobChIN.Services;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web.PublishedCache;
using System.Collections.Generic;
using System;
using Mlok.Core.Utils;
using Mlok.Core.Models;
using Mlok.Web.Sites.JobChIN.Members;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Mlok.Core.Services;
using Mlok.Core.Services.OC;
using Mlok.Core;
using Umbraco.Core;
using Mlok.Core.Models.ApiExceptions;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;

namespace Mlok.Web.Sites.JobChIN
{
    [CustomModuleClass("JobChIN")]
    public class JobChINModule : CustomWebDataModule, IJobChINModule, IOCOrdersProcessor
    {
        public ISettings Settings { get; private set; }

        public LogService LogService { get; private set; }
        public JobChINMembersPlugin MembersPlugin => this.GetPlugin<JobChINMembersPlugin>();
        WebDataMembersPlugin IWebDataMembersModule.MembersPlugin => MembersPlugin;
        public string AngularModuleName => "JobChINModule";

        public AreaOfInterestController AreaOfInterestController { get; }
        public HardSkillController HardSkillController { get; }
        public HardSkillSuggestController HardSkillSuggestController { get; }
        public SoftSkillController SoftSkillController { get; }
        public WorkerCountRangeController WorkerCountRangeController { get; }
        public CountryController CountryController { get; }
        public LocalAdministrativeUnitsController LocalAdministrativeUnitsController { get; }
        public FacultyController FacultyController { get; }
        public LanguageController LanguageController { get; }
        public CompanyTypeController CompanyTypeController { get; }
        public ContractTypeController ContractTypeController { get; }

        public CompanyService CompanyService { get; private set; }
        public StudentService StudentService { get; private set; }
        public StudentPhotoService StudentPhotoService { get; private set; }
        public WorkPositionService WorkPositionService { get; private set; }
        public CvService CvService { get; private set; }

        public JobChINModule(SiteHelper helper, IPublishedContent moduleNode) : base(helper, moduleNode)
        {
            Settings = new Settings(helper, ModuleNode.Children.FirstOrDefault(x => x.DocumentTypeAlias == SiteConstants.ModuleSettingsAlias));

            AreaOfInterestController = new AreaOfInterestController(ScopeProvider);
            HardSkillController = new HardSkillController(ScopeProvider);
            HardSkillSuggestController = new HardSkillSuggestController(ScopeProvider);
            SoftSkillController = new SoftSkillController(ScopeProvider);
            WorkerCountRangeController = new WorkerCountRangeController(ScopeProvider);
            CountryController = new CountryController(ScopeProvider);
            LocalAdministrativeUnitsController = new LocalAdministrativeUnitsController(ScopeProvider);
            FacultyController = new FacultyController();
            LanguageController = new LanguageController(ScopeProvider);
            CompanyTypeController = new CompanyTypeController(ScopeProvider);
            ContractTypeController = new ContractTypeController(ScopeProvider);
        }

        public override void InitializePlugins()
        {
            base.InitializePlugins();
            WorkPositionService = new WorkPositionService(ScopeProvider, Settings);
            var compnayFileService = new CompanyFileService(ScopeProvider, WebCentrumContext.WebCentrumService, Services.MediaService, Site.MediaFolderId);
            var aresService = new AresService();
            CompanyService = new CompanyService(ScopeProvider, MembersPlugin, Settings, WebCentrumContext.OCService, compnayFileService, aresService, WorkPositionService);
            var studentFileService = new StudentFileService(ScopeProvider, WebCentrumContext.WebCentrumService, Services.MediaService, Site.MediaFolderId);
            StudentService = new StudentService(ScopeProvider, MembersPlugin, Settings, studentFileService);
            StudentPhotoService = new StudentPhotoService(ScopeProvider, MembersPlugin, StudentService, CompanyService);
            CvService = new CvService(ScopeProvider, Settings, Services.MediaService);
        }

        public override void SetupTreeItemsResolver(WebDataTreeResolver cfg)
        {
            cfg.SetRootItem("JobCheckIN systém", "icon-whhg-workshirt", itemCfg =>
            {
                itemCfg.SetChildrenResolver((parent, list) =>
                {
                    list.AddChild("company", "Zaměstnavatelé", "icon-company", settingCfg =>
                    {
                        settingCfg.AddTab("Zaměstnavatelé", CompanyService.CompanyController.GetPaged, new CompanyFilter() { IsConfirmed = true }, CompanyService.CompanyController.GetModelId(), tabCfg =>
                        {
                            tabCfg.EnableRecordsDetail(CompanyService.CompanyController.GetById, x => CompanyService.Update(x));
                            tabCfg.CanCreate = true;


                            tabCfg.AddRecordsActionWithResult("Anonymizovat záznamy", CompanyService.AnonymizeUsers);
                            tabCfg.AddRecordsAction<SendUserInvitaion>("Pozvat nového uživatele", CompanyService.InviteUser);
                        });

                        settingCfg.AddTab("Nepotvrzení", CompanyService.CompanyController.GetPaged, new CompanyFilter() { IsConfirmed = false }, CompanyService.CompanyController.GetModelId(), tabCfg =>
                        {
                            tabCfg.EnableRecordsDetail(CompanyService.CompanyController.GetById, company => CompanyService.Update(company));

                            tabCfg.CanCreate = true;
                            tabCfg.AddRecordsActionWithResult("Potvrdit", CompanyService.ConfirmCompanies);
                            tabCfg.AddRecordsActionWithResult("Odstranit", CompanyService.DeleteCompanies, "icon-delete")
                                .WithConfirmation("Opravdu chcete odstranit vybrané firmy?");
                        });


                        settingCfg.AddTab("Archivované", CompanyService.CompanyController.GetPaged, new CompanyFilter() { Archived = true }, CompanyService.CompanyController.GetModelId());
                    });

                    list.AddChild("workPosition", "Pracovní pozice", "icon-document", settingCfg =>
                    {
                        settingCfg.AddTab("Aktivní", WorkPositionService.WorkPositionController.GetPaged, new WorkPositionFilter() { IncludeCompany = true, IncludeStats = true, Hidden = false, Active = true }, WorkPositionService.WorkPositionController.GetModelId(), tabCfg =>
                        {
                            tabCfg.EnableRecordsDetail(WorkPositionService.WorkPositionController.GetById, x => WorkPositionService.Update(x));
                            tabCfg.CanCreate = true;
                        });

                        settingCfg.AddTab("Expirované", WorkPositionService.WorkPositionController.GetPaged, new WorkPositionFilter() { IncludeCompany = true, IncludeStats = true, Hidden = false, Active = false }, WorkPositionService.WorkPositionController.GetModelId());

                        settingCfg.AddTab("Skryté", WorkPositionService.WorkPositionController.GetPaged, new WorkPositionFilter() { IncludeCompany = true, IncludeStats = true, Hidden = true }, WorkPositionService.WorkPositionController.GetModelId());

                    });

                    list.AddChild("students", "Studenti", "icon-graduate", settingCfg =>
                    {
                        settingCfg.AddTab("Aktivní", StudentService.StudentController.GetPaged, new StudentFilter() { Active = true }, StudentService.StudentController.GetModelId(), tabCfg =>
                        {
                            tabCfg.EnableRecordsDetail(StudentService.StudentController.GetById, x => StudentService.Update(x));
                            tabCfg.AddRecordsActionWithResult("Anonymizovat záznamy", StudentService.AnonymizeUsers);
                            tabCfg.AddRecordsDownload("Stáhnout životopisy", DownloadCvs);
                        });

                        settingCfg.AddTab("Archivované", StudentService.StudentController.GetPaged, new StudentFilter() { Active = false }, StudentService.StudentController.GetModelId());
                    });

                    list.AddChild("hardSkills", "Tvrdé dovednosti", "icon-playing-cards", settingCfg =>
                    {
                        settingCfg.AddTab("Tvrdé dovednosti", HardSkillController.GetPaged, new HardSkillFilter(), HardSkillController.GetModelId(), tabCfg =>
                        {
                            tabCfg.EnableRecordsDetail(HardSkillController.GetById, HardSkillController.Update);
                            tabCfg.CanCreate = true;
                            tabCfg.AddRecordsActionWithResult("Odstranit", DeleteItems(HardSkillController), "icon-delete")
                                .WithConfirmation("Opravdu chcete odstranit vybrané záznamy?");
                        });

                        settingCfg.AddTab("Navrhnuté", HardSkillSuggestController.GetPaged, HardSkillSuggestController.GetModelId(), tabCfg =>
                        {
                            tabCfg.AddRecordsActionWithResult("Odstranit", DeleteItems(HardSkillSuggestController), "icon-delete")
                                .WithConfirmation("Opravdu chcete odstranit vybrané záznamy?");

                            tabCfg.AddRecordsActionWithResult<HardSkill>("Potvrdit", CreateHardSkill, icon: "icon-arrow-right");
                        });
                    });

                    list.AddChild("softSkills", "Měkké dovednosti", "icon-conversation-alt", settingCfg =>
                    {
                        settingCfg.AddTab("Měkké dovednosti", SoftSkillController.GetPaged, SoftSkillController.GetModelId(), tabCfg =>
                        {
                            tabCfg.EnableRecordsDetail(SoftSkillController.GetById, SoftSkillController.Update);
                            tabCfg.CanCreate = true;
                            tabCfg.AddRecordsActionWithResult("Odstranit", DeleteItems(SoftSkillController), "icon-delete")
                                .WithConfirmation("Opravdu chcete odstranit vybrané záznamy?");
                        });
                    });

                    list.AddChild("settings", "Nastavení", "icon-settings", settingCfg =>
                    {
                        settingCfg.AddTab("Typy účtů", CompanyTypeController.GetPaged, CompanyTypeController.GetModelId(), tabCfg =>
                        {
                            tabCfg.EnableRecordsDetail(CompanyTypeController.GetById, CompanyTypeController.Update);
                            tabCfg.CanCreate = true;
                        });

                        settingCfg.AddTab("Oblasti zájmu", AreaOfInterestController.GetPaged, AreaOfInterestController.GetModelId(), tabCfg =>
                        {
                            tabCfg.EnableRecordsDetail(AreaOfInterestController.GetById, AreaOfInterestController.Update);
                            tabCfg.CanCreate = true;
                            tabCfg.AddRecordsActionWithResult("Odstranit", DeleteItems(AreaOfInterestController), "icon-delete")
                                .WithConfirmation("Opravdu chcete odstranit vybrané záznamy?");
                        });

                        settingCfg.AddTab("Jazyky", LanguageController.GetPaged, LanguageController.GetModelId(), tabCfg =>
                        {
                            tabCfg.EnableRecordsDetail(LanguageController.GetById, LanguageController.Update);
                            tabCfg.CanCreate = true;
                        });

                        settingCfg.AddTab("Počet zaměstnanců", WorkerCountRangeController.GetPaged, WorkerCountRangeController.GetModelId(), tabCfg =>
                        {
                            tabCfg.EnableRecordsDetail(WorkerCountRangeController.GetById, WorkerCountRangeController.Update);
                            tabCfg.CanCreate = true;
                        });
                    });
                });
            });
        }

        private WebDataDownloadResult DownloadCvs(IEnumerable<int> ids)
        {
            var students = StudentService.StudentController.GetByIds(ids);
            var model = CvService.GetDocxCvs(students);
            return new WebDataDownloadResult(model.Content, model.MimeType, $"cvs.{model.FileExt}");
        }

        public override bool ShowInTree() => true;

        WebDataRecordsActionWithResult<TId> DeleteItems<TModel, TId>(IModelController<TModel, TId> controller)
        {
            return (IEnumerable<TId> ids) =>
            {
                controller.Delete(ids);
                return new WebDataActionResult($"Hotovo!");
            };
        }

        WebDataActionResult CreateHardSkill(IEnumerable<int> ids, HardSkill config)
        {
            if (ids.Count() > 1)
                return new WebDataActionResult($"Musí být vybrána pouze jedna tvrdá dovednost!");

            HardSkillController.Update(config);
            return new WebDataActionResult($"Hotovo!");
        }

        public IEnumerable<EnumerablePickerValue<Sector?, string>> SectorPicker
            => Enum.GetValues(typeof(Sector)).Cast<Sector>().Select(y => EnumerablePickerValue.From<Sector?, string>(y, EnumUtils.GetDisplayName(y)));

        public MembersPluginConfig GetMembersConfig()
        {
            return new MembersPluginConfig()
            {
                GetAngularProfileConfig = GetAngularProfileConfig,
                GetAngularConfig = GetAngularConfig,
                OnMemberValidated = OnMemberValidated,
                OnMemberDeleting = OnMemberDeleting,
                HasLocalAccounts = true,
                LoginProviders = (member) => member == null || MembersPlugin.IsStudent(member.Id)
                    ? new ExternalLoginProviderConfig(WebCentrumConstants.Security.MuniProviderId, false).AsEnumerableOfOne()
                    : Enumerable.Empty<ExternalLoginProviderConfig>(),
                ValidateUser = ValidateUser,
                AllowAccountDelete = true,
                UserMenuItems = GetUserMenuItems,
            };
        }

        IEnumerable<PackageModel> GetPackages(IEnumerable<CompanyType> companyTypes)
        {
            var result = new List<PackageModel>();
            foreach (var companyType in companyTypes)
            {
                if (companyType.PackageProfit.HasValue)
                    result.Add(new PackageModel()
                    {
                        Id = companyType.PackageProfit.Value,
                        Name = companyType.Name,
                        Price = companyType.PriceProfit.Value,
                        Currency = Currency.CZK,
                    });
                if (companyType.PackageNonProfit.HasValue)
                    result.Add(new PackageModel()
                    {
                        Id = companyType.PackageNonProfit.Value,
                        Name = companyType.Name,
                        Price = companyType.PriceNonProfit.Value,
                        Currency = Currency.CZK,
                    });
            }
            return result;
        }

        public Result<CompanyRegistrationConfigDto> CompanyRegistrationConfig()
        {
            var result = new CompanyRegistrationConfigDto()
            {
                GdprUrl = Settings.CompanyGDPR,
            };
            return new Result<CompanyRegistrationConfigDto>(result);
        }

        public Result<StudentRegistrationConfigDto> StudentRegistrationConfig()
        {
            var result = new StudentRegistrationConfigDto()
            {
                Text = Settings.StudentRegistrationText,
                GdprUrl = Settings.StudentGDPR,
            };
            return new Result<StudentRegistrationConfigDto>(result);
        }

        public Result SuggestHardSkill(string name)
        {
            if (name == null || name.Length > WebDataConstants.MaximumNameLength)
                return new Result(new BadRequestException("Field 'Name' is too long"));
            var suggest = new HardSkillSuggest()
            {
                Name = name,
                MemberId = MembersPlugin.GetCurrentMember().Id,
            };
            HardSkillSuggestController.Update(suggest);
            return new Result();
        }

        public object GetOffersConfig()
        {
            return new
            {
                Settings.CompanyNoLogoLink,
                AreaOfInterests = AreaOfInterestController.GetAll<AreaOfInterestDto>(),
                LocalAdministrativeUnits = LocalAdministrativeUnitsController.GetPicker(),
                Languages = LanguageController.GetAll<LanguageDto>(),
                ContractTypes = ContractTypeController.GetAll<ContractTypeDto>(),
            };
        }

        public object GetCompanyListConfig()
        {
            return new
            {
                Settings.CompanyNoLogoLink,
                AreaOfInterests = AreaOfInterestController.GetAll<AreaOfInterestDto>(),
            };
        }

        private void OnMemberDeleting(MemberDeletingEventArgs obj)
        {
            if (MembersPlugin.IsStudent(obj.Member.Id))
                StudentService.DeleteUserByMemberId(obj.Member.Id);
            else
                CompanyService.DeleteUserByMemberId(obj.Member.Id);

        }

        private object GetAngularConfig()
        {
            return new AngularConfigDto()
            {
                StudentAfterLogin = Settings.StudentAfterLogin,
                AreaOfInterests = AreaOfInterestController.GetAll<AreaOfInterestDto>(),
                HardSkills = HardSkillController.GetAll<HardSkillDto>(),
                SoftSkills = SoftSkillController.GetAll<SoftSkillDto>(),
                WorkerCountRanges = WorkerCountRangeController.GetAll<WorkerCountRangeDto>(),
                Countries = CountryController.GetPicker(),
                LocalAdministrativeUnits = LocalAdministrativeUnitsController.GetPicker(),
                Faculties = FacultyController.GetPicker(),
                Languages = LanguageController.GetAll<LanguageDto>(),
                ContractTypes = ContractTypeController.GetAll<ContractTypeDto>(),
            };
        }

        private AngularProfileDto GetAngularProfileConfig(MemberPublishedContent arg)
        {
            bool isStudent = MembersPlugin.IsStudent(arg.Id);
            CompanyAngularConfigDto companyConfig = null;
            if (!isStudent)
            {
                var companyUser = CompanyService.GetCurrent(arg.Id);
                Company company = CompanyService.CompanyController.GetById(companyUser.CompanyId);
                companyConfig = Mapper.Map<CompanyAngularConfigDto>(company);
                companyConfig.User = Mapper.Map<CompanyUserUpdateDto>(companyUser);
                companyConfig.Role = companyUser.Role;
                companyConfig.MemberId = companyUser.MemberId;
                companyConfig.CompanyTypeConfig = CompanyTypeConfig(company);
                companyConfig.MaxDuration = Settings.WorkPositionMaxDuration;
            }

            StudentAngularConfigDto studentConfig = null;
            if (isStudent)
            {
                Student student = StudentService.GetCurrentDetailed(arg.Id);
                studentConfig = GetStudentConfig(student);
            }


            return new AngularProfileDto
            {
                Company = companyConfig,
                Student = studentConfig,
            };
        }


        private IEnumerable<string> ValidateUser(MemberPublishedContent arg)
        {
            if (MembersPlugin.IsStudent(arg.Id))
            {
                var student = StudentService.GetCurrentDetailed(arg.Id);
                if (student.EndAccess() <= DateTime.Now)
                    return this.Localize("Váš přístup do systému Vám byl odebrán, protože již nejste studentem MUNI", "") // TODO: translate
                        .AsEnumerableOfOne();
            }
            return Enumerable.Empty<string>();
        }

        private IEnumerable<UserMenuItem> GetUserMenuItems(MemberPublishedContent arg)
        {
            int unreadNotifications = 0;
            if (MembersPlugin.IsStudent(arg.Id))
            {
                return new UserMenuItem[] {
                    new UserMenuItem() { Name = this.Localize("Profil", "Profile"), Route = Settings.RouteStudentCV },
                    new UserMenuItem() { Name = this.Localize("Moje nabídky", "My offers"), Route = Settings.RouteMyOffers },
                    new UserMenuItem() { Name = $"{this.Localize("Notifikace", "Notification")} ({unreadNotifications})", Route = Settings.RouteNotifications },
                };
            }

            var result = new List<UserMenuItem>();
            if (CompanyService.GetCurrent().Role == Role.CompanyAdmin)
            {
                result.Add(new UserMenuItem() { Name = this.Localize("O firmě", "About company"), Route = Settings.RouteCompanyAboutUs });
                result.Add(new UserMenuItem() { Name = this.Localize("Vyhledávání studentů", "Student search"), Route = Settings.RouteCompanyStudentSearch});
            }
            result.Add(new UserMenuItem() { Name = this.Localize("Moje nabídky", "My offers"), Route = Settings.RouteMyOffers });
            result.Add(new UserMenuItem() { Name = $"{this.Localize("Notifikace", "Notification")} ({unreadNotifications})", Route = Settings.RouteNotifications });

            return result;

        }

        public StudentAngularConfigDto GetStudentConfig(Student student)
        {
            var profile = Mapper.Map<StudentAngularConfigDto>(student);
            profile.FormDescriptions = Mapper.Map<FormDescriptionsDto>(Settings);
            return profile;
        }

        private CompanyTypeConfigDto CompanyTypeConfig(Company company)
        {
            var comapnyTypes = CompanyTypeController.GetAll();
            var orders = WebCentrumContext.OCService.GetAngularOrderState(GetPackages(comapnyTypes), company.CompanyTypes?.Select(x => x.OCRequestId.HasValue ? x.OCRequestId.Value : 0).Where(x => x > 0) ?? Enumerable.Empty<int>());

            return new CompanyTypeConfigDto()
            {
                AfterLogin = Settings.CompanyAfterLogin,
                CurrentCompanyTypes = company.CompanyTypes?.Select(x =>
                {
                    var current = Mapper.Map<CompanyCompanyTypeDto>(x);
                    current.OrderState = orders.FirstOrDefault(order => order.OrderRequestId == x.OCRequestId);
                    return current;
                }),
                CompanyTypes = comapnyTypes.Where(x => x.Visible).Select(Mapper.Map<CompanyTypeDto>),
            };
        }

        public void UpdateOrdersStatus(IEnumerable<int> paidRequestIds, IEnumerable<int> cancelledRequestIds) => CompanyService.UpdateOrdersStatus(paidRequestIds, cancelledRequestIds);

        public OCReturnRedirectMeta GetOCReturnRedirect(int requestId, int orderId, bool isPaid) => CompanyService.GetOCReturnRedirect(requestId, orderId, isPaid);
    }
}