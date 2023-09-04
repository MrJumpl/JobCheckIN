using System;
using System.Linq.Expressions;
using Mlok.Core.Data;
using Mlok.Core.Services;
using Mlok.Web.Sites.JobChIN.Models;
using Mlok.Web.Sites.JobChIN.Models.StudentModels;
using Umbraco.Core.Services;

namespace Mlok.Web.Sites.JobChIN.Services
{
    public class StudentFileService : FileService<Student, JobChIN_StudentFile, FileCategory>
    {
        public StudentFileService(DbScopeProvider scopeProvider, WebCentrumService webCentrumService, IMediaService mediaService, int rootFolder)
            : base(scopeProvider, webCentrumService, mediaService, rootFolder)
        { }

        protected override Expression<Func<JobChIN_StudentFile, object>> GetFileIdExpression => file => file.FileId;

        protected override string GetFolderName(Student model) => $"Student_{model.Uco}";

        protected override bool HasCategory(JobChIN_StudentFile model, FileCategory category) => category.HasFlag((FileCategory)model.Category);
    }
}