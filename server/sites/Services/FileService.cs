using AutoMapper;
using Mlok.Core;
using Mlok.Core.Data;
using Mlok.Core.Services;
using Mlok.Web.Sites.JobChIN.Controllers;
using Mlok.Web.Sites.JobChIN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Core.Services;
using UmbracoConstants = Umbraco.Core.Constants;

namespace Mlok.Web.Sites.JobChIN.Services
{

    public abstract class FileService<TModel, TDBModel, TCategory> : IFileService<TModel, TCategory>
        where TModel : IUserMedia
    {
        private readonly WebCentrumService webCentrumService;
        private readonly IMediaService mediaService;
        private readonly int rootFolder;
        private readonly FileController<TDBModel> fileController;

        protected FileService(DbScopeProvider scopeProvider, WebCentrumService webCentrumService, IMediaService mediaService, int rootFolder)
        {
            this.webCentrumService = webCentrumService;
            this.mediaService = mediaService;
            this.rootFolder = rootFolder;

            fileController = new FileController<TDBModel>(scopeProvider, GetFileIdExpression);
        }
        
        public void UpdateFiles(TCategory category, TModel current, TModel old)
        {
            var newValues = Mapper.Map<IEnumerable<TDBModel>>(current).Where(x => HasCategory(x, category));
            var oldValues = Mapper.Map<IEnumerable<TDBModel>>(old).Where(x => HasCategory(x, category)).Select(x => GetFileId(x));

            DeleteFiles(oldValues.Except(newValues.Select(x => GetFileId(x))));
            InsertFiles(current.MediaFolderId, newValues.Where(x => !oldValues.Contains(GetFileId(x))));
        }

        public int CreateFolder(TModel model)
        {
            var media = mediaService.CreateMedia(GetFolderName(model), rootFolder, UmbracoConstants.Conventions.MediaTypes.Folder);
            mediaService.Save(media);
            model.MediaFolderId = media.Id;
            return media.Id;
        }

        public void DeleteFolder(TModel model)
        {
            var folder = mediaService.GetById(model.MediaFolderId);
            mediaService.Delete(folder);
        }

        /// <summary>
        /// Returns the name of the media folder when it is creating. Must return unique name for each user.
        /// </summary>
        protected abstract string GetFolderName(TModel model);
        /// <summary>
        /// Check if the database model belongs to category.
        /// </summary>
        protected abstract bool HasCategory(TDBModel model, TCategory category);
        /// <summary>
        /// Expression for the file id from the database model.
        /// </summary>
        protected abstract Expression<Func<TDBModel, object>> GetFileIdExpression { get; }

        void DeleteFiles(IEnumerable<int> fileIds)
        {
            bool deleteAny = false;
            foreach (var id in fileIds)
            {
                var media = mediaService.GetById(id);
                if (media == null)
                    continue;
                mediaService.Delete(media);

                deleteAny = true;
            }
            if (deleteAny)
                fileController.DeleteFiles(fileIds);
        }

        void InsertFiles(int folderId, IEnumerable<TDBModel> files)
        {
            bool insertAny = false;
            foreach (var file in files)
            {
                var media = mediaService.GetById(GetFileId(file));
                if (media.ParentId == WebCentrumConstants.SystemConstants.TempMediaFolderId)
                    webCentrumService.PersistTempImage(media, folderId);
                if (media.ParentId != folderId)
                    mediaService.Move(media, folderId);
                insertAny = true;
            }
            if (insertAny)
                fileController.InsertFiles(files);
        }

        int GetFileId(TDBModel model) => (int)GetFileIdExpression.Compile().Invoke(model);
    }
}