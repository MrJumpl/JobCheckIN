using AutoMapper;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using Umbraco.Core;
using Umbraco.Core.Models.Mapping;
using UmbracoConstants = Umbraco.Core.Constants;

namespace Mlok.Web.Sites.JobChIN.Models.Mapping
{
    public class PresentationMapper : MapperConfiguration
    {
        public override void ConfigureMappings(IConfiguration config, ApplicationContext applicationContext)
        {
            config.CreateMap<Presentation, PresentationDto>()
                .ForMember(d => d.Logo, _ => _.MapFrom(s => GetFileUploadDto(applicationContext, s.Logo)))
                .ForMember(d => d.BackgroundImage, _ => _.MapFrom(s => GetFileUploadDto(applicationContext, s.BackgroundImage)))
                .ReverseMap()
                .ForMember(d => d.Logo, _ => _.MapFrom(s => s.Logo == null ? default(int?) : s.Logo.Id))
                .ForMember(d => d.BackgroundImage, _ => _.MapFrom(s => s.BackgroundImage == null ? default(int?) : s.BackgroundImage.Id));
        }

        FileUploadDto GetFileUploadDto(ApplicationContext applicationContext, int? id)
        {
            if (!id.HasValue)
                return null;
            var media = applicationContext.Services.MediaService.GetById(id.Value);
            if (media == null)
                return null;
            return new FileUploadDto()
            {
                Id = media.Id,
                Name = media.Name,
                Size = media.GetValue<int>(UmbracoConstants.Conventions.Media.Bytes) / 1024,
            };
        }
    }
}