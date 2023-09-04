using AutoMapper;
using Mlok.Core.Data;
using Mlok.Core.Services.OC;
using Mlok.Core.Utils;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Mlok.Web.Sites.JobChIN.Models.Filters;
using Mlok.Web.Sites.JobChIN.Utils;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models.Mapping;

namespace Mlok.Web.Sites.JobChIN.Models.Mapping
{
    public partial class CompanyMapper : MapperConfiguration
    {
        public override void ConfigureMappings(IConfiguration config, ApplicationContext applicationContext)
        {
            config.CreateMap<CompanyCreateDto, Company>();

            config.CreateMap<CompanyCreateDto, CompanyUser>();

            config.CreateMap<CompanyUpdateDto, Company>()
                .ReverseMap();

            config.CreateMap<CompanyUserUpdateDto, CompanyUser>()
                .ReverseMap();

            config.CreateMap<GeneralInfo, JobChIN_Company>()
                .ReverseMap()
                .AfterMap((s, d) => 
                {
                    d.GdprAgreement = true;
                });

            config.CreateMap<ContactPerson, JobChIN_CompanyUser>()
                .ReverseMap();

            config.CreateMap<NotificationSettings, JobChIN_CompanyUser>()
                .ReverseMap();

            config.CreateMap<Candidates, JobChIN_Company>()
                .ReverseMap()
                .AfterMap((s, d) => 
                {
                    if (s.HasJoinedData<JobChIN_CompanyAreaOfInterest>())
                        d.AreaOfInterests = s.Joined<JobChIN_CompanyAreaOfInterest>().Select(x => x.AreaOfInterestId).Distinct();
                    if (s.HasJoinedData<JobChIN_CompanyLanguageSkillPreferred>())
                        d.LanguageSkillPrefered = s.Joined<JobChIN_CompanyLanguageSkillPreferred>().Select(x => new LanguageModel() {
                            LanguageId = x.LanguageId,
                            Skill = (LanguageSkill)x.SkillLevel,
                        })
                        .DistinctBy(x => x.LanguageId.Value);
                    if (s.HasJoinedData<JobChIN_CompanyFaculty>())
                        d.Faculties = s.Joined<JobChIN_CompanyFaculty>().Select(x => x.FacultyId).Distinct();
                });

            config.CreateMap<CompanyCompanyType, JobChIN_CompanyCompanyType>()
                .ReverseMap();

            config.CreateMap<Branch, JobChIN_CompanyBranch>()
                .ReverseMap();

            config.CreateMap<JobChIN_Company, CompanyBranches>()
                .AfterMap((s, d) =>
                {
                    if (s.HasJoinedData<JobChIN_CompanyBranch>())
                        d.Branches = s.Joined<JobChIN_CompanyBranch>().Select(Mapper.Map<Branch>).DistinctBy(x => x.BranchId);
                });

            config.CreateMap<CompanyCompanyType, CompanyCompanyTypeDto>();

            config.CreateMap<Company, IEnumerable<JobChIN_CompanyAreaOfInterest>>()
                .ConstructUsing((Company company) =>
                {
                    return company.Candidates?.AreaOfInterests?.Select(x => new JobChIN_CompanyAreaOfInterest()
                    {
                        CompanyId = company.CompanyId,
                        AreaOfInterestId = x,
                    }) ?? Enumerable.Empty<JobChIN_CompanyAreaOfInterest>();
                });

            config.CreateMap<Company, IEnumerable<JobChIN_CompanyLanguageSkillPreferred>>()
                .ConstructUsing((Company company) =>
                {
                    return company.Candidates?.LanguageSkillPrefered?.Select(x => new JobChIN_CompanyLanguageSkillPreferred()
                    {
                        CompanyId = company.CompanyId,
                        LanguageId = x.LanguageId.Value,
                        SkillLevel = (int)x.Skill,
                    }) ?? Enumerable.Empty<JobChIN_CompanyLanguageSkillPreferred>();
                });

            config.CreateMap<Company, IEnumerable<JobChIN_CompanyFaculty>>()
                .ConstructUsing((Company company) =>
                {
                    return company.Candidates?.Faculties?.Select(x => new JobChIN_CompanyFaculty()
                    {
                        CompanyId = company.CompanyId,
                        FacultyId = x,
                    }) ?? Enumerable.Empty<JobChIN_CompanyFaculty>();
                });

            config.CreateMap<Company, IEnumerable<JobChIN_CompanyCompanyType>>()
                .ConstructUsing((Company company) =>
                {
                    return company.CompanyTypes
                        ?.Select(x => Mapper.Map(x, new JobChIN_CompanyCompanyType() { CompanyId = company.CompanyId }))
                        ?? Enumerable.Empty<JobChIN_CompanyCompanyType>();
                });

            config.CreateMap<Company, IEnumerable<JobChIN_CompanyFile>>()
                .ConstructUsing(GetFiles);

            config.CreateMap<Company, IEnumerable<JobChIN_CompanyBranch>>()
                .ConstructUsing((Company company) =>
                {
                    return company.Branches?.Branches
                        ?.Select(x => Mapper.Map(x, new JobChIN_CompanyBranch() { CompanyId = company.CompanyId }))
                        ?? Enumerable.Empty<JobChIN_CompanyBranch>();
                });

            config.CreateMap<Presentation, JobChIN_Company>()
                .ReverseMap()
                .AfterMap((s, d) =>
                {
                    if (s.HasJoinedData<JobChIN_CompanyFile>())
                    {
                        d.Logo = s.Joined<JobChIN_CompanyFile>().FirstOrDefault(x => x.Category == (int)FileCategory.Logo)?.FileId;
                        d.BackgroundImage = s.Joined<JobChIN_CompanyFile>().FirstOrDefault(x => x.Category == (int)FileCategory.BackgroundImage)?.FileId;
                    }
                });

            config.CreateMap<Company, JobChIN_Company>()
                .AfterMap((s, d) => 
                {
                    Mapper.Map(s.GeneralInfo, d);
                    Mapper.Map(s.Candidates, d);
                    Mapper.Map(s.Presentation, d);
                })
                .ReverseMap()
                .AfterMap((s, d) => 
                {
                    d.GeneralInfo = Mapper.Map<GeneralInfo>(s);
                    d.Candidates = Mapper.Map<Candidates>(s);
                    d.Presentation = Mapper.Map<Presentation>(s);
                    d.Branches = Mapper.Map<CompanyBranches>(s);
                    if (s.HasJoinedData<JobChIN_CompanyUser>())
                        d.Users = s.Joined<JobChIN_CompanyUser>().Select(Mapper.Map<CompanyUser>).DistinctBy(x => x.MemberId);

                    if (s.HasJoinedData<JobChIN_CompanyCompanyType>())
                        d.CompanyTypes = s.Joined<JobChIN_CompanyCompanyType>().Select(Mapper.Map<CompanyCompanyType>).DistinctBy(x => x.CompanyCompanyTypeId);
                    if (s.HasJoinedData<JobChIN_WorkPosition>())
                        d.WorkPositionCount = s.Joined<JobChIN_WorkPosition>().Count();
                });

            config.CreateMap<CompanyUser, JobChIN_CompanyUser>()
                .AfterMap((s, d) =>
                {
                    Mapper.Map(s.ContactPerson, d);
                    Mapper.Map(s.NotificationSettings, d);
                    
                    d.Phone = PhoneNumberUtils.Format(d.Phone);
                })
                .ReverseMap()
                .AfterMap((s, d) =>
                {
                    d.ContactPerson = Mapper.Map<ContactPerson>(s);
                    d.NotificationSettings = Mapper.Map<NotificationSettings>(s);
                });


            config.CreateMap<Company, CompanyAngularConfigDto>()
                .ForMember(d => d.Model, _ => _.MapFrom(s => Mapper.Map<CompanyUpdateDto>(s)));

            config.CreateMap<CompanyUser, OrderRequestModel>()
                .ForMember(d => d.Firstname, _ => _.MapFrom(s => s.ContactPerson.Firstname))
                .ForMember(d => d.Surname, _ => _.MapFrom(s => s.ContactPerson.Surname))
                .ForMember(d => d.Email, _ => _.MapFrom(s => s.Member.Email));

            config.CreateMap<Company, OrderRequestModel>()
                .ForMember(d => d.FACompanyName1, _ => _.MapFrom(s => s.GeneralInfo.CompanyName))
                .ForMember(d => d.FAIC, _ => _.MapFrom(s => s.GeneralInfo.Ico.ToString()))
                .ForMember(d => d.FACountry, _ => _.MapFrom(s => s.GeneralInfo.CountryId))
                .ForMember(d => d.FACity, _ => _.MapFrom(s => s.GeneralInfo.City))
                .ForMember(d => d.FAStreet, _ => _.MapFrom(s => s.GeneralInfo.Street))
                .ForMember(d => d.FAPSC, _ => _.MapFrom(s => s.GeneralInfo.ZipCode))
                .AfterMap((s, d) => 
                {
                    if (!s.GeneralInfo.Dic.IsNullOrWhiteSpace())
                        d.FADIC = s.GeneralInfo.Dic;
                    if (s.GeneralInfo.CorrespondenceCountryId.HasValue)
                    {
                        d.KONCountry = s.GeneralInfo.CorrespondenceCountryId;
                        d.KONCity = s.GeneralInfo.CorrespondenceCity;
                        d.KONStreet = s.GeneralInfo.CorrespondenceStreet;
                        d.KONPSC = s.GeneralInfo.CorrespondenceZipCode;
                    }
                });

            config.CreateMap<Company, StudentCompanyListViewDto>()
                .ForMember(d => d.CompanyName, _ => _.MapFrom(s => s.GeneralInfo.CompanyName))
                .ForMember(d => d.Logo, _ => _.MapFrom(s => s.Presentation.Logo))
                .AfterMap((s, d) => 
                {
                    d.Group = char.ToUpper(StrUtils.RemoveDiacritics(d.CompanyName.Substring(0, 1))[0]);
                });

            config.CreateMap<CompanyUser, CompanyUserViewDto>();

            config.CreateMap<CompanyFilterDto, CompanyFilter>();

            config.CreateMap<SendUserInvitaion, InviteCompanyUserModel>();
        }

        public IEnumerable<JobChIN_CompanyFile> GetFiles(Company company)
        {
            if (company.Presentation?.Logo != null)
            {
                yield return new JobChIN_CompanyFile()
                {
                    CompanyId = company.CompanyId,
                    FileId = company.Presentation.Logo.Value,
                    Category = (int)FileCategory.Logo,
                };
            }
            if (company.Presentation?.BackgroundImage != null)
            {
                yield return new JobChIN_CompanyFile()
                {
                    CompanyId = company.CompanyId,
                    FileId = company.Presentation.BackgroundImage.Value,
                    Category = (int)FileCategory.BackgroundImage,
                };
            }

        }
    }
}