using AutoMapper;
using Mlok.Core.Data;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Umbraco.Core;
using Umbraco.Core.Models.Mapping;

namespace Mlok.Web.Sites.JobChIN.Models.Mapping
{
    public class SoftSkillMapper : MapperConfiguration
    {
        public override void ConfigureMappings(IConfiguration config, ApplicationContext applicationContext)
        {
            config.CreateMap<JobChIN_SoftSkill, SoftSkill>()
                .TranslateFromDB(d => d.Name, _ => _.TranslateFrom(s => s.NameCs, "cs"), _ => _.TranslateFrom(s => s.NameEn, "en"))
                .TranslateFromDB(d => d.Description, _ => _.TranslateFrom(s => s.DescriptionCs, "cs"), _ => _.TranslateFrom(s => s.DescriptionEn, "en"))
                .ReverseMap()
                .TranslateToDB(s => s.Name, _ => _.TranslateTo(x => x.NameCs, "cs"), _ => _.TranslateTo(s => s.NameEn, "en"))
                .TranslateToDB(s => s.Description, _ => _.TranslateTo(x => x.DescriptionCs, "cs"), _ => _.TranslateTo(s => s.DescriptionEn, "en"));


            config.CreateMap<SoftSkill, SoftSkillDto>()
                .ReverseMap();
        }
    }

    public class MatchMapper : MapperConfiguration
    {
        public override void ConfigureMappings(IConfiguration config, ApplicationContext applicationContext)
        {
            config.CreateMap<JobChIN_Match, MatchCategory>()
                .ConstructUsing(match =>
                {
                    bool isSuitable = match.Suitable > 0;
                    if (isSuitable && match.OverallMatch > 7)
                        return MatchCategory.Recommend;
                    if (isSuitable && match.OverallMatch > 0)
                        return MatchCategory.Match;
                    if (isSuitable && match.OverallMatch == 0)
                        return MatchCategory.NoMatch;

                    return MatchCategory.NotSuitable;
                });
        }
    }
}
