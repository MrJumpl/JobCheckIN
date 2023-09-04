using AutoMapper;
using Mlok.Core.Data;
using Mlok.Core.Utils;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Mlok.Web.Sites.JobChIN.Models.StudentModels;
using Mlok.Web.Sites.JobChIN.Utils;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models.Mapping;

namespace Mlok.Web.Sites.JobChIN.Models.Mapping
{
    public class StudentMapper : MapperConfiguration
    {
        public override void ConfigureMappings(IConfiguration config, ApplicationContext applicationContext)
        {
            config.CreateMap<Studijni_Pomery, Study>()
                .ForMember(d => d.From, _ => _.MapFrom(s => s.Od))
                .ForMember(d => d.To, _ => _.MapFrom(s => s.Do))
                .ForMember(d => d.Faculty, _ => _.ResolveUsing(s => 
                {
                    var faculty = s.Single<Pracoviste>();
                    return this.Localize(faculty.Cesky_Nazev, faculty.Anglicky_Nazev);
                }))
                .ForMember(d => d.Fields, _ => _.ResolveUsing(s =>
                {
                    if (!s.HasJoinedData<Studijni_Pomery_Obory>())
                        return Enumerable.Empty<string>();
                    
                    return s.Joined<Studijni_Pomery_Obory>()
                        .Select(x => x.Single<Studium_Obory>())
                        .DistinctBy(x => x.Kod_Obor)
                        .Select(x => this.Localize(x.Nazev_cs, x.Nazev_en));
                }))
                .AfterMap((s, d) => 
                {
                    var programme = s.Single<Studium_Programy>();
                    d.Programme = this.Localize(programme.Nazev_cs, programme.Nazev_en);
                    d.Degree = programme.Titul;
                });

            config.CreateMap<BasicInfo, JobChIN_Student>()
                .ReverseMap()
                .AfterMap((s, d) =>
                {
                    if (s.HasJoinedData<JobChIN_StudentAreaOfInterest>())
                    {
                        d.AreaOfInterests = s.Joined<JobChIN_StudentAreaOfInterest>().Where(x => x.IsPrimary).Select(x => x.AreaOfInterestId).Distinct();
                        d.SecondaryAreaOfInterests = s.Joined<JobChIN_StudentAreaOfInterest>().Where(x => !x.IsPrimary).Select(x => x.AreaOfInterestId).Distinct();
                    }
                    if (s.HasJoinedData<JobChIN_StudentPreferredContractType>())
                        d.ContractTypes = s.Joined<JobChIN_StudentPreferredContractType>().Select(x => x.ContractTypeId).Distinct();
                });

            config.CreateMap<StudentPhoto, JobChIN_Student>()
                .ReverseMap()
                .AfterMap((s, d) =>
                {
                    if (s.HasJoinedData<JobChIN_StudentFile>())
                        d.Photo = s.Joined<JobChIN_StudentFile>().FirstOrDefault(x => x.Category == (int)FileCategory.Photo)?.FileId;
                });

            config.CreateMap<NotificationSettings, JobChIN_Student>()
                .ReverseMap();

            config.CreateMap<Visibility, JobChIN_Student>()
                .ReverseMap();

            config.CreateMap<WorkExperiences, JobChIN_Student>()
               .ReverseMap()
                .AfterMap((s, d) =>
                {
                    if (s.HasJoinedData<JobChIN_StudentWorkExperience>())
                        d.Experiences = s.Joined<JobChIN_StudentWorkExperience>().DistinctBy(x => x.WorkExperienceId).Select(x => Mapper.Map<WorkExperience>(x));
                });

            config.CreateMap<JobChIN_StudentSoftSkill, StudentSoftSkill>()
                .ReverseMap();

            config.CreateMap<JobChIN_Student, Studies>()
                .AfterMap((s, d) =>
                {
                    if (s.HasJoinedData<JobChIN_StudentOtherStudy>())
                        d.Others = s.Joined<JobChIN_StudentOtherStudy>().Select(Mapper.Map<OtherStudy>).DistinctBy(x => x.OtherUniversityStudyId);
                })
                .ReverseMap();

            config.CreateMap<JobChIN_Student, Skills>()
                .AfterMap((s, d) =>
                {
                    if (s.HasJoinedData<JobChIN_StudentHardSkill>())
                        d.HardSkills = s.Joined<JobChIN_StudentHardSkill>().Select(x => x.HardSkillId).Distinct();
                    if (s.HasJoinedData<JobChIN_StudentLanguageSkill>())
                        d.Languages = s.Joined<JobChIN_StudentLanguageSkill>().Select(x => new LanguageModel()
                        {
                            LanguageId = x.LanguageId,
                            Skill = (LanguageSkill)x.SkillLevel,
                        })
                        .DistinctBy(x => x.LanguageId.Value);
                    if (s.HasJoinedData<JobChIN_StudentSoftSkill>())
                        d.SoftSkills = s.Joined<JobChIN_StudentSoftSkill>().Select(Mapper.Map<StudentSoftSkill>).DistinctBy(x => x.SoftSkillId);
                    if (s.HasJoinedData<JobChIN_StudentFile>())
                    {
                        d.EducationCertificate = s.Joined<JobChIN_StudentFile>().FirstOrDefault(x => x.Category == (int)FileCategory.EducationCertificate)?.FileId;
                        d.LanguageCertificate = s.Joined<JobChIN_StudentFile>().FirstOrDefault(x => x.Category == (int)FileCategory.LanguageCertificate)?.FileId;
                    }
                });


            config.CreateMap<OtherStudy, JobChIN_StudentOtherStudy>()
                .ReverseMap();


            config.CreateMap<Contact, JobChIN_Student>()
                .AfterMap((s, d) =>
                {
                    d.Phone = PhoneNumberUtils.Format(d.Phone);
                })
               .ReverseMap();

            config.CreateMap<Student, IEnumerable<JobChIN_StudentAreaOfInterest>>()
               .ConstructUsing((Student student) =>
               {
                   var primary = student.BasicInfo?.AreaOfInterests?.Select(x => new JobChIN_StudentAreaOfInterest()
                   {
                       StudentId = student.StudentId,
                       IsPrimary = true,
                       AreaOfInterestId = x,
                   }) ?? Enumerable.Empty<JobChIN_StudentAreaOfInterest>();
                   var secondary = student.BasicInfo?.SecondaryAreaOfInterests?.Select(x => new JobChIN_StudentAreaOfInterest()
                   {
                       StudentId = student.StudentId,
                       IsPrimary = false,
                       AreaOfInterestId = x,
                   }) ?? Enumerable.Empty<JobChIN_StudentAreaOfInterest>();
                   return primary.Concat(secondary);
               });

            config.CreateMap<Student, IEnumerable<JobChIN_StudentPreferredContractType>>()
               .ConstructUsing((Student student) =>
               {
                   return student.BasicInfo?.ContractTypes?.Select(x => new JobChIN_StudentPreferredContractType()
                   {
                       StudentId = student.StudentId,
                       ContractTypeId = x,
                   }) ?? Enumerable.Empty<JobChIN_StudentPreferredContractType>();
               });

            config.CreateMap<Student, IEnumerable<JobChIN_StudentHardSkill>>()
               .ConstructUsing((Student student) =>
               {
                    return student.Skills?.HardSkills?.Select(x => new JobChIN_StudentHardSkill()
                        {
                            StudentId = student.StudentId,
                            HardSkillId = x,
                        }) ?? Enumerable.Empty<JobChIN_StudentHardSkill>();
               });

            config.CreateMap<Student, IEnumerable<JobChIN_StudentSoftSkill>>()
               .ConstructUsing((Student student) =>
               {
                    return student.Skills?.SoftSkills
                        ?.Select(x => Mapper.Map(x, new JobChIN_StudentSoftSkill() { StudentId = student.StudentId }))
                        ?? Enumerable.Empty<JobChIN_StudentSoftSkill>();
               });

            config.CreateMap<Student, IEnumerable<JobChIN_StudentLanguageSkill>>()
                .ConstructUsing((Student student) =>
                {
                    return student.Skills?.Languages?.Select(x => new JobChIN_StudentLanguageSkill()
                    {
                        StudentId = student.StudentId,
                        LanguageId = x.LanguageId.Value,
                        SkillLevel = (int)x.Skill,
                    }) ?? Enumerable.Empty<JobChIN_StudentLanguageSkill>();
                });

            config.CreateMap<Student, IEnumerable<JobChIN_StudentFile>>()
                .ConstructUsing(GetFiles);

            config.CreateMap<Student, JobChIN_Student>()
                .AfterMap((s, d) =>
                {
                    d.MemberId = s.Member?.Id ?? 0;
                    Mapper.Map(s.BasicInfo, d);
                    Mapper.Map(s.Photo, d);
                    Mapper.Map(s.WorkExperiences, d);
                    Mapper.Map(s.Contact, d);
                    Mapper.Map(s.Studies, d);
                    Mapper.Map(s.NotificationSettings, d);
                    Mapper.Map(s.Visibility, d);
                })
                .ReverseMap()
                .AfterMap((s, d) =>
                {
                    var person = s.SingleOrDefault<Osoby>();
                    if (person != null)
                    {
                        d.FullName = person.GetGivenNameFamilyName(true);
                        d.CvFileName = StrUtils.RemoveDiacritics(person.GetFamilyNameGivenName(namesCommaSeparated: false)).Replace(" ", "_");
                        d.MuniStudies = person.Joined<Studijni_Pomery>()
                            .DistinctBy(x => x.ID)
                            .Select(Mapper.Map<Study>)
                            .OrderByDescending(x => x.From);
                    }

                    d.BasicInfo = Mapper.Map<BasicInfo>(s);
                    d.Photo = Mapper.Map<StudentPhoto>(s);
                    d.WorkExperiences = Mapper.Map<WorkExperiences>(s);
                    d.Studies = Mapper.Map<Studies>(s);
                    d.Skills = Mapper.Map<Skills>(s);
                    d.Contact = Mapper.Map<Contact>(s);
                    d.NotificationSettings = Mapper.Map<NotificationSettings>(s);
                    d.Visibility = Mapper.Map<Visibility>(s);

                    if (s.HasJoinedData<JobChIN_Match>())
                        d.Match = Mapper.Map<MatchCategory>(s.Joined<JobChIN_Match>().First());
                    if (s.HasJoinedData<JobChIN_StudentShownInterest>())
                    {
                        d.ShownInterest = s.Joined<JobChIN_StudentShownInterest>().FirstOrDefault()?.Date;
                        d.IsRevealedByCompany = true;
                    }
                    if (s.HasJoinedData<JobChIN_CompanyStudentRevealed>())
                        d.IsRevealedByCompany = true;
                });

            config.CreateMap<Settings, FormDescriptionsDto>()
                .ForMember(d => d.Skills, _ => _.MapFrom(s => s.StudentSkillsDescription))
                .ForMember(d => d.CareerVision, _ => _.MapFrom(s => s.CareerVisionDescription))
                .ForMember(d => d.Presentation, _ => _.MapFrom(s => s.PresentationDescription))
                .ForMember(d => d.CareerPortfolio, _ => _.MapFrom(s => s.CareerPortfolioDescription))
                .ForMember(d => d.AditionalEducation, _ => _.MapFrom(s => s.AditionalEducationDescription))
                .ForMember(d => d.Visibility, _ => _.MapFrom(s => s.Visibility));

            config.CreateMap<Student, StudentAngularConfigDto>()
                .ForMember(d => d.Model, _ => _.MapFrom(s => Mapper.Map<StudentUpdateDto>(s)));

            config.CreateMap<StudentUpdateDto, Student>()
                .ReverseMap();

            config.CreateMap<Student, WorkPositionStudentListViewDto>()
                .AfterMap((s, d) =>
                {
                    if (s.IsRevealedByCompany)
                        d.PhotoLink = Mapper.Map<int?, FileUploadDto>(s.Photo?.Photo)?.Link;
                    else
                    {
                        d.FullName = this.Localize("(Neodhalen)", "(Not revealed)");
                    }
                });
        }

        public IEnumerable<JobChIN_StudentFile> GetFiles(Student student)
        {
            if (student.Photo?.Photo != null)
            {
                yield return new JobChIN_StudentFile()
                {
                    StudentId = student.StudentId,
                    FileId = student.Photo.Photo.Value,
                    Category = (int)FileCategory.Photo,
                };
            }
            if (student.Skills?.EducationCertificate != null)
            {
                yield return new JobChIN_StudentFile()
                {
                    StudentId = student.StudentId,
                    FileId = student.Skills.EducationCertificate.Value,
                    Category = (int)FileCategory.EducationCertificate,
                };
            }
            if (student.Skills?.LanguageCertificate != null)
            {
                yield return new JobChIN_StudentFile()
                {
                    StudentId = student.StudentId,
                    FileId = student.Skills.LanguageCertificate.Value,
                    Category = (int)FileCategory.LanguageCertificate,
                };
            }

        }
    }
}