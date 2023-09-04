using Mlok.Core.Data;
using Mlok.Web.Sites.JobChIN.Models;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public abstract class StudentJoinController<TChildId, TDBModel> : JoinController<Student, int, TChildId, TDBModel>
    {
        protected StudentJoinController(DbScopeProvider scopeProvider) : base(scopeProvider)
        {
        }

        protected override string ParentIdLabel => "StudentId";

        protected override int GetParentId(Student parent) => parent.StudentId;
    }
}