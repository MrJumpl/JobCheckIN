using AutoMapper;
using Mlok.Core.Data;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Mlok.Web.Sites.JobChIN.Models.Filters;
using Mlok.Web.Sites.JobChIN.Models.WorkPositionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models.Mapping;

namespace Mlok.Web.Sites.JobChIN.Models.Mapping
{
    public class WorkPositionMapper : MapperConfiguration
    {
        public override void ConfigureMappings(IConfiguration config, ApplicationContext applicationContext)
        {

            config.CreateMap<WorkPosition, IEnumerable<JobChIN_WorkPositionContractType>>()
                .ConstructUsing((WorkPosition workPosition) =>
                {
                    return workPosition.BasicInfo?.ContractTypes?.Select(x => new JobChIN_WorkPositionContractType()
                    {
                        WorkPositionId = workPosition.WorkPositionId,
                        ContractTypeId = x,
                    }) ?? Enumerable.Empty<JobChIN_WorkPositionContractType>();
                });

            config.CreateMap<WorkPosition, IEnumerable<JobChIN_WorkPositionUser>>()
                .ConstructUsing((WorkPosition workPosition) =>
                {
                    return workPosition.BasicInfo?.Users?.Select(x => new JobChIN_WorkPositionUser()
                    {
                        WorkPositionId = workPosition.WorkPositionId,
                        CompanyUserId = x,
                    }) ?? Enumerable.Empty<JobChIN_WorkPositionUser>();
                });

            config.CreateMap<WorkPosition, IEnumerable<JobChIN_WorkPositionAreaOfInterest>>()
                .ConstructUsing((WorkPosition workPosition) =>
                {
                    return workPosition.Candidates?.AreaOfInterests?.Select(x => new JobChIN_WorkPositionAreaOfInterest()
                    {
                        WorkPositionId = workPosition.WorkPositionId,
                        AreaOfInterestId = x,
                    }) ?? Enumerable.Empty<JobChIN_WorkPositionAreaOfInterest>();
                });

            config.CreateMap<WorkPosition, IEnumerable<JobChIN_WorkPositionHardSkill>>()
                .ConstructUsing((WorkPosition workPosition) =>
                {
                    return workPosition.Candidates?.HardSkills?.Select(x => new JobChIN_WorkPositionHardSkill()
                    {
                        WorkPositionId = workPosition.WorkPositionId,
                        HardSkillId = x,
                    }) ?? Enumerable.Empty<JobChIN_WorkPositionHardSkill>();
                });

            config.CreateMap<WorkPosition, IEnumerable<JobChIN_WorkPositionSoftSkill>>()
                .ConstructUsing((WorkPosition workPosition) =>
                {
                    return workPosition.Candidates?.SoftSkills?.Select(x => new JobChIN_WorkPositionSoftSkill()
                    {
                        WorkPositionId = workPosition.WorkPositionId,
                        SoftSkillId = x,
                    }) ?? Enumerable.Empty<JobChIN_WorkPositionSoftSkill>();
                });

            config.CreateMap<WorkPosition, IEnumerable<JobChIN_WorkPositionFaculty>>()
                .ConstructUsing((WorkPosition workPosition) =>
                {
                    return workPosition.Candidates?.Faculties?.Select(x => new JobChIN_WorkPositionFaculty()
                    {
                        WorkPositionId = workPosition.WorkPositionId,
                        FacultyId = x,
                    }) ?? Enumerable.Empty<JobChIN_WorkPositionFaculty>();
                });

            config.CreateMap<WorkPosition, IEnumerable<JobChIN_WorkPositionLanguageSkill>>()
                .ConstructUsing((WorkPosition workPosition) =>
                {
                    return workPosition.Candidates?.Languages?.Select(x => new JobChIN_WorkPositionLanguageSkill()
                    {
                        WorkPositionId = workPosition.WorkPositionId,
                        LanguageId = x.LanguageId.Value,
                        SkillLevel = (int)x.Skill,
                        Optional = x.Optional
                    }) ?? Enumerable.Empty<JobChIN_WorkPositionLanguageSkill>();
                });

            config.CreateMap<Visibility, JobChIN_WorkPosition>()
                .ReverseMap();

            config.CreateMap<BasicInfo, JobChIN_WorkPosition>()
                .ReverseMap()
                .AfterMap((s, d) =>
                {
                    if (s.HasJoinedData<JobChIN_WorkPositionContractType>())
                        d.ContractTypes = s.Joined<JobChIN_WorkPositionContractType>().Select(x => x.ContractTypeId).Distinct();
                    d.CurrentLocationId = s.LocationId;
                    if (s.HasJoinedData<JobChIN_CompanyBranch>())
                        d.CurrentLocationId = s.Single<JobChIN_CompanyBranch>().LocationId;
                    if (s.HasJoinedData<JobChIN_WorkPositionUser>())
                        d.Users = s.Joined<JobChIN_WorkPositionUser>().Select(x => x.CompanyUserId).Distinct();
                    else
                        d.Users = Enumerable.Empty<int>();
                });

            config.CreateMap<Detail, JobChIN_WorkPosition>()
                .ReverseMap();

            config.CreateMap<Candidates, JobChIN_WorkPosition>()
                .ReverseMap()
                .AfterMap((s, d) =>
                {
                    if (s.HasJoinedData<JobChIN_WorkPositionAreaOfInterest>())
                        d.AreaOfInterests = s.Joined<JobChIN_WorkPositionAreaOfInterest>().Select(x => x.AreaOfInterestId).Distinct();
                    if (s.HasJoinedData<JobChIN_WorkPositionHardSkill>())
                        d.HardSkills = s.Joined<JobChIN_WorkPositionHardSkill>().Select(x => x.HardSkillId).Distinct();
                    if (s.HasJoinedData<JobChIN_WorkPositionSoftSkill>())
                        d.SoftSkills = s.Joined<JobChIN_WorkPositionSoftSkill>().Select(x => x.SoftSkillId).Distinct();
                    if (s.HasJoinedData<JobChIN_WorkPositionFaculty>())
                        d.Faculties = s.Joined<JobChIN_WorkPositionFaculty>().Select(x => x.FacultyId).Distinct();
                    if (s.HasJoinedData<JobChIN_WorkPositionLanguageSkill>())
                        d.Languages = s.Joined<JobChIN_WorkPositionLanguageSkill>().DistinctBy(x => x.LanguageId).Select(x => new WorkPositionLanguageModel()
                        {
                            LanguageId = x.LanguageId,
                            Skill = (LanguageSkill)x.SkillLevel,
                            Optional = x.Optional,
                        });
                });

            config.CreateMap<CandidateRequest, JobChIN_WorkPosition>()
                .ReverseMap();

            config.CreateMap<WorkPosition, JobChIN_WorkPosition>()
                .AfterMap((s, d) =>
                {
                    Mapper.Map(s.Visibility, d);
                    Mapper.Map(s.BasicInfo, d);
                    Mapper.Map(s.Detail, d);
                    Mapper.Map(s.Candidates, d);
                    Mapper.Map(s.CandidateRequest, d);

                    if (s.Candidates != null)
                    {
                        d.HasAreaOfInterest = s.Candidates.AreaOfInterests?.Any() ?? false;
                        d.HasFaculty = s.Candidates.Faculties?.Any() ?? false;
                        d.HasMandatoryLangs = s.Candidates.Languages?.Any(x => !x.Optional) ?? false;
                        d.AreaOfInterestCount = s.Candidates.AreaOfInterests?.Count() ?? 0;
                        d.HardSkillsCount = s.Candidates.HardSkills?.Count() ?? 0;
                        d.SoftSkillsCount = s.Candidates.SoftSkills?.Count() ?? 0;
                        d.LanguageOptionalSkillsCount = s.Candidates.Languages?.Count(x => x.Optional) ?? 0;

                        if (d.SoftSkillsCount > 3)
                            d.SoftSkillsCount = 3;
                        if (d.AreaOfInterestCount > 3)
                            d.AreaOfInterestCount = 3;
                    }
                })
                .ReverseMap()
                .AfterMap((s, d) =>
                {
                    d.Visibility = Mapper.Map<Visibility>(s);
                    d.BasicInfo = Mapper.Map<BasicInfo>(s);
                    d.Detail = Mapper.Map<Detail>(s);
                    d.Candidates = Mapper.Map<Candidates>(s);
                    d.CandidateRequest = Mapper.Map<CandidateRequest>(s);

                    if (s.HasJoinedData<JobChIN_Company>())
                    {
                        var company = s.Joined<JobChIN_Company>().FirstOrDefault();
                        d.CompanyName = company.CompanyName;
                        if (company.HasJoinedData<JobChIN_CompanyFile>())
                            d.CompanyLogo = company.Joined<JobChIN_CompanyFile>().FirstOrDefault()?.FileId;
                    }
                    if (s.HasJoinedData<JobChIN_StudentShownInterest>())
                        d.ShownInterestCount = s.Joined<JobChIN_StudentShownInterest>().Select(x => x.StudentId).Distinct().Count();
                    if (s.HasJoinedData<JobChIN_StudentWorkPositionViewed>())
                        d.ViewsCount = s.Joined<JobChIN_StudentWorkPositionViewed>().Select(x => x.StudentId).Distinct().Count();
                    if (s.HasJoinedData<JobChIN_Match>())
                        d.Match = Mapper.Map<MatchCategory>(s.Joined<JobChIN_Match>().First());
                    d.StudentFavorite = s.HasJoinedData<JobChIN_StudentWorkPositionFollowed>();
                });

            config.CreateMap<WorkPosition, WorkPositionCreateDto>()
                .ReverseMap();

            config.CreateMap<WorkPosition, WorkPositionUpdateDto>()
                .ReverseMap();

            config.CreateMap<WorkPosition, CompanyWorkPositionListViewDto>()
                .ForMember(d => d.Name, _ => _.MapFrom(s => s.BasicInfo.Name))
                .ForMember(d => d.Publication, _ => _.MapFrom(s => s.BasicInfo.Publication))
                .ForMember(d => d.Expiration, _ => _.MapFrom(s => s.BasicInfo.Expiration));

            config.CreateMap<WorkPosition, StudentWorkPositionListViewDto>()
                .ForMember(d => d.Name, _ => _.MapFrom(s => s.BasicInfo.Name))
                .ForMember(d => d.Publication, _ => _.MapFrom(s => s.BasicInfo.Publication))
                .ForMember(d => d.Expiration, _ => _.MapFrom(s => s.BasicInfo.Expiration))
                .ForMember(d => d.Remote, _ => _.MapFrom(s => s.BasicInfo.Remote))
                .ForMember(d => d.ContractTypes, _ => _.MapFrom(s => s.BasicInfo.ContractTypes))
                .ForMember(d => d.LocationId, _ => _.MapFrom(s => s.BasicInfo.CurrentLocationId))
                .ForMember(d => d.Favorite, _ => _.MapFrom(s => s.StudentFavorite));

            config.CreateMap<SearchStudentFilterDto, WorkPosition>()
                .AfterMap((s, d) =>
                {
                    d.BasicInfo = Mapper.Map<BasicInfo>(s);
                    d.Detail = new Detail()
                    {
                        Description = string.Empty,
                        Requesting = string.Empty,
                    };
                    d.Candidates = Mapper.Map<Candidates>(s);
                    d.Visibility = new Visibility()
                    {
                        Hidden = true,
                    };
                });

            config.CreateMap<SearchStudentFilterDto, BasicInfo>()
                .AfterMap((s, d) =>
                {
                    d.Name = string.Empty;
                    d.Language = "cs";
                    d.Remote = Remote.No;
                });

            config.CreateMap<SearchStudentFilterDto, Candidates>();

            config.CreateMap<WorkPositionFilterDto, WorkPositionFilter>();
        }
    }
}