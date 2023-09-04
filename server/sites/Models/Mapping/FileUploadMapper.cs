using AutoMapper;
using Mlok.Core.Utils;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using System;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Models.Mapping;
using UmbracoConstants = Umbraco.Core.Constants;

namespace Mlok.Web.Sites.JobChIN.Models.Mapping
{
    public class FileUploadMapper : MapperConfiguration
    {
        public override void ConfigureMappings(IConfiguration config, ApplicationContext applicationContext)
        {
            config.CreateMap<int?, FileUploadDto>()
                .ConstructUsing(s =>
                {
                    return GetFileUploadDto(applicationContext, s);
                })
                .ReverseMap()
                .ConstructUsing(s =>
                {
                    return s?.Id;
                });

            config.CreateMap<int?, string>()
                .ConstructUsing(s =>
                {
                    return GetLinkDto(applicationContext, s);
                });
        }

        FileUploadDto GetFileUploadDto(ApplicationContext applicationContext, int? id)
        {
            var media = GetMedia(applicationContext, id);
            if (media == null)
                return null;
            return new FileUploadDto()
            {
                Id = media.Id,
                Name = media.Name,
                Size = media.GetValue<int>(UmbracoConstants.Conventions.Media.Bytes) / 1024,
                Link = GetMediaLink(media),
            };
        }

        public static string GetLinkDto(ApplicationContext applicationContext, int? id)
        {
            var media = GetMedia(applicationContext, id);
            if (media == null)
                return null;
            return GetMediaLink(media);
        }

        private static IMedia GetMedia(ApplicationContext applicationContext, int? id)
        {
            if (!id.HasValue)
                return null;
            return applicationContext.Services.MediaService.GetById(id.Value);
        }

        private static string GetMediaLink(IMedia media) => media.GetValue<string>(UmbracoConstants.Conventions.Media.File);
    }
}