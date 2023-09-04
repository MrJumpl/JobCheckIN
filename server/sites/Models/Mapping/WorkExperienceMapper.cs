using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Mlok.Core.Data;
using Mlok.Web.Sites.JobChIN.Models.StudentModels;
using Umbraco.Core;
using Umbraco.Core.Models.Mapping;

namespace Mlok.Web.Sites.JobChIN.Models.Mapping
{
    public class WorkExperienceMapper : MapperConfiguration
    {
        public override void ConfigureMappings(IConfiguration config, ApplicationContext applicationContext)
        {
            config.CreateMap<WorkExperience, JobChIN_StudentWorkExperience>()
                .ReverseMap();
        }
    }
}