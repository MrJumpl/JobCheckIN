using AutoMapper;
using Mlok.Core.Data;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Umbraco.Core.Persistence;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class CompanyBranchController : MultiJoinController<int, Branch, JobChIN_CompanyBranch>
    {
        private readonly WorkPositionController workPositionController;

        public CompanyBranchController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
            workPositionController = new WorkPositionController(scopeProvider);
        }

        public IEnumerable<Branch> GetByCompanyId(int companyId) => JobChIN_CompanyBranch.SelectFromDB()
            .Where(x => x.CompanyId == companyId)
            .Execute()
            .Select(Mapper.Map<Branch>)
            .ToList();

        public Branch GetByCompanyId(int companyId, int branchId)
        {
            var model = GetBranchByCompanyId(companyId, branchId);
            if (model != null)
                return Mapper.Map<Branch>(model);
            return null;

        }

        public bool HasCompanyBranch(int companyId, int branchId) => GetBranchByCompanyId(companyId, branchId) != null;

        public override Expression<Func<Branch, int>> GetModelId() => branch => branch.BranchId;

        protected override JobChIN_CompanyBranch GetDBModel(int parentId)
        {
            return new JobChIN_CompanyBranch() { CompanyId = parentId };
        }

        protected override IEnumerable<JobChIN_CompanyBranch> GetOldValues(UmbracoDatabase database, int parentId)
        {
            return JobChIN_CompanyBranch.SelectFromDB(database)
                   .Where(x => x.CompanyId == parentId)
                   .Execute()
                   .ToList();
        }

        protected override bool SameId(Branch model, JobChIN_CompanyBranch dbModel)
        {
            return model.BranchId == dbModel.BranchId;
        }

        protected override void Delete(UmbracoDatabase database, JobChIN_CompanyBranch model)
        {
            base.Delete(database, model);
            workPositionController.UpdateLocation(model.BranchId, model.LocationId);
        }

        JobChIN_CompanyBranch GetBranchByCompanyId(int companyId, int branchId)
        {
            using (var scope = ScopeProvider.CreateReadOnlyScope())
            {
                return JobChIN_CompanyBranch.SelectFromDB(scope.Database)
                    .Where(x => x.CompanyId == companyId)
                    .Where(x => x.BranchId == branchId)
                    .SingleOrDefault();
            }
        }
    }
}