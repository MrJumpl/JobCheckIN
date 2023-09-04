using AutoMapper;
using Mlok.Core.Data;
using Umbraco.Core;
using Umbraco.Core.Models.Mapping;


namespace Mlok.Web.Sites.JobChIN.Models.Mapping
{
    public class ShowInterestMapper : MapperConfiguration
    {
        public override void ConfigureMappings(IConfiguration config, ApplicationContext applicationContext)
        {
            config.CreateMap<ShowInterest, JobChIN_StudentShownInterest>();
        }
    }
}