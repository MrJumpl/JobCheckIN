using AutoMapper;
using Mlok.Core.Data;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Umbraco.Core;
using Umbraco.Core.Models.Mapping;

namespace Mlok.Web.Sites.JobChIN.Models.Mapping
{
    public class WorkerCountRangeMapper : MapperConfiguration
    {
        public override void ConfigureMappings(IConfiguration config, ApplicationContext applicationContext)
        {
            config.CreateMap<JobChIN_WorkerCountRange, WorkerCountRange>()
                .ReverseMap();

            config.CreateMap<WorkerCountRange, WorkerCountRangeDto>()
                .ForMember(d => d.Name, _ => _.ResolveUsing(s => s.GetName()));
        }
    }
}