using System;
using System.Linq.Expressions;
using Mlok.Core.Data;
using Mlok.Core.Services;
using Mlok.Web.Sites.JobChIN.Models;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using Umbraco.Core.Services;

namespace Mlok.Web.Sites.JobChIN.Services
{
    public class CompanyFileService : FileService<Company, JobChIN_CompanyFile, FileCategory>
    {
        public CompanyFileService(DbScopeProvider scopeProvider, WebCentrumService webCentrumService, IMediaService mediaService, int rootFolder) 
            : base(scopeProvider, webCentrumService, mediaService, rootFolder)
        {
        }

        protected override Expression<Func<JobChIN_CompanyFile, object>> GetFileIdExpression => file => file.FileId;

        protected override string GetFolderName(Company model) => model.GeneralInfo?.CompanyName ?? $"Firma_{model.CompanyId}";

        protected override bool HasCategory(JobChIN_CompanyFile model, FileCategory category) => category.HasFlag((FileCategory)model.Category);
    }
}