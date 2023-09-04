using AutoMapper;
using Mlok.Core.Data;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Umbraco.Core;
using Umbraco.Core.Models.Mapping;

namespace Mlok.Web.Sites.JobChIN.Models.Mapping
{
    public class CompanyTypeMapper : MapperConfiguration
    {
        public override void ConfigureMappings(IConfiguration config, ApplicationContext applicationContext)
        {
            config.CreateMap<JobChIN_CompanyType, CompanyType>()
                .TranslateFromDB(d => d.Name, _ => _.TranslateFrom(s => s.NameCs, "cs"), _ => _.TranslateFrom(s => s.NameEn, "en"))
                .ReverseMap()
                .TranslateToDB(s => s.Name, _ => _.TranslateTo(x => x.NameCs, "cs"), _ => _.TranslateTo(s => s.NameEn, "en"));

            config.CreateMap<CompanyType, CompanyTypeDto>();

            config.CreateMap<CompanyType, CompanyCompanyType>();
        }
    }
}