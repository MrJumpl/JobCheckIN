using AutoMapper;
using Mlok.Core.Data;
using Mlok.Core.Utils;
using Mlok.Web.Sites.JobChIN.Models.CompanyModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public class CompanyCompanyTypeController
    {
        private static readonly string[] PAID_COLUMNS = new[] { "Confirmed", "Paid" };

        protected DbScopeProvider ScopeProvider { get; }

        public CompanyCompanyTypeController(DbScopeProvider scopeProvider)
        {
            ScopeProvider = scopeProvider;
        }

        public IEnumerable<CompanyCompanyType> GetByRequestIds(IEnumerable<int> ids) => JobChIN_CompanyCompanyType.SelectFromDB()
            .Where(x => x.OCRequestId, SqlCompareType.In, ids.Cast<int?>())
            .Execute()
            .Select(Mapper.Map<CompanyCompanyType>)
            .ToList();

        public IEnumerable<CompanyCompanyType> GetCompanyActive(int companyId, DateTime to) => JobChIN_CompanyCompanyType.SelectFromDB()
            .Where(x => x.ActiveFrom < to)
            .Where(x => x.ActiveTo >= DateTime.Now)
            .Where(x => x.CompanyId == companyId)
            .Execute()
            .Select(Mapper.Map<CompanyCompanyType>)
            .ToList();

        public bool ValidateRequestId(int companyId, int requestId) => JobChIN_CompanyCompanyType.SelectFromDB()
            .Where(x => x.CompanyId == companyId)
            .Where(x => x.OCRequestId == requestId)
            .SingleOrDefault() != null;

        public void Add(int companyId, CompanyCompanyType child)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                scope.Database.Insert(Mapper.Map(child, new JobChIN_CompanyCompanyType() { CompanyId = companyId }));
                scope.Complete();
            }
        }

        public void Pay(CompanyCompanyType child)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                child.Confirmed = true;
                child.Paid = true;
                scope.Database.Update(Mapper.Map<JobChIN_CompanyCompanyType>(child), PAID_COLUMNS);
                scope.Complete();
            }
        }

        public void Cancel(CompanyCompanyType child)
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                scope.Database.Delete<JobChIN_CompanyCompanyType>(child.CompanyCompanyTypeId);
                scope.Complete();
            }
        }

        public IEnumerable<CompanyCompanyType> Update(int companyId, IEnumerable<CompanyCompanyType> childs)
        {
            var result = new List<CompanyCompanyType>();
            using (var scope = ScopeProvider.CreateScope())
            {
                var oldValues = JobChIN_CompanyCompanyType.SelectFromDB(scope.Database)
                    .Where(x => x.CompanyId == companyId)
                    .LeftJoin<OC_Order>(cfg => cfg.On((cct, order) => cct.OCRequestId == order.Request_ID))
                    .Execute()
                    .ToList();

                foreach (var oldValue in oldValues)
                {
                    if (oldValue.HasJoinedData<OC_Order>())
                    {
                        var order = oldValue.Joined<OC_Order>().First();
                        if (order.Canceled)
                            scope.Database.Delete(oldValue);
                        oldValue.Paid = order.Paid;
                    }
                    var newValue = childs.FirstOrDefault(x => x.CompanyCompanyTypeId == oldValue.CompanyCompanyTypeId);
                    if (newValue == null)
                        scope.Database.Delete(oldValue);
                    else
                    {
                        newValue.Paid = oldValue.Paid || newValue.Paid;
                        newValue.Confirmed = newValue.Paid || newValue.Confirmed;
                        scope.Database.Update(Mapper.Map(newValue, new JobChIN_CompanyCompanyType() { CompanyId = companyId }));
                        result.Add(newValue);
                    }
                }
                foreach (var newValue in childs.Where(x => x.CompanyCompanyTypeId == default(int)))
                {
                    var newModel = Mapper.Map(newValue, new JobChIN_CompanyCompanyType() { CompanyId = companyId });
                    scope.Database.Insert(newModel);
                    newValue.CompanyCompanyTypeId = newModel.CompanyCompanyTypeId;
                    result.Add(newValue);
                }

                scope.Complete();
            }
            return result;
        }
    }
}