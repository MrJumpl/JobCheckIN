using AutoMapper;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Mlok.Web.Sites.JobChIN.Models.StudentModels;
using Umbraco.Core;
using Umbraco.Core.Models.Mapping;


namespace Mlok.Web.Sites.JobChIN.Models.Mapping
{
    public class SkillsMapper : MapperConfiguration
    {
        public override void ConfigureMappings(IConfiguration config, ApplicationContext applicationContext)
        {
            config.CreateMap<Skills, SkillsDto>()
                .ReverseMap();
        }
    }
}