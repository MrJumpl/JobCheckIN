using Mlok.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class FileController<TDBModel>
    {
        private readonly Expression<Func<TDBModel, object>> fileIdExpression;

        protected DbScopeProvider ScopeProvider { get; }

        public FileController(DbScopeProvider scopeProvider, Expression<Func<TDBModel, object>> fileIdExpression)
        {
            ScopeProvider = scopeProvider;
            this.fileIdExpression = fileIdExpression;
        }

        public void DeleteFiles(IEnumerable<int> fileIds)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var sqlBase = Sql.Builder
                    .WhereIn(fileIdExpression, fileIds, new SqlServerSyntaxProvider());
                scope.Database.Delete<TDBModel>(sqlBase);
                scope.Complete();
            }
        }

        public void InsertFiles(IEnumerable<TDBModel> models)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                foreach (var model in models)
                    scope.Database.Insert(model);
                scope.Complete();
            }
        }
    }
}