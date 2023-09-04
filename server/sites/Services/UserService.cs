using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Members;
using Mlok.Web.Sites.JobChIN.Models;
using System.Collections.Generic;
using Umbraco.Web.PublishedCache;

namespace Mlok.Web.Sites.JobChIN.Services
{
    public abstract class UserService<T> 
        where T : class, IUser
    {
        protected readonly JobChINMembersPlugin membersPlugin;

        protected UserService(JobChINMembersPlugin membersPlugin)
        {
            this.membersPlugin = membersPlugin;
        }

        /// <summary>
        /// Returns the user model that is cached. If the member id is null then returns currently logged in user.
        /// </summary>
        /// <param name="memberId">Member id of the user.</param>
        /// <returns></returns>
        public T GetCurrent(int? memberId = null)
        {
            var model = membersPlugin.GetStoredModel<T>(ClassIdentifier);
            if (model != null && (!memberId.HasValue || memberId.Value == model.Member.Id))
                return model;
            if (memberId.HasValue)
            {
                bool? isStudent = membersPlugin.IsStoredStudent(memberId.Value);
                if (isStudent.HasValue && isStudent.Value != IsStudent)
                    return default(T);
            }

            MemberPublishedContent member = null;
            if (memberId.HasValue)
                member = membersPlugin.GetMember(memberId.Value);
            else
                member = membersPlugin.GetCurrentMember();
            if (member == null)
                return default(T);

            model = GetByMemberId(member.Id);
            if (model != null)
            {
                model.Member = member;
                membersPlugin.StoreCurrentModel(model, ClassIdentifier, IsStudent);
            }
            return model;
        }

        /// <summary>
        /// Anonymize users by admin.
        /// </summary>
        /// <param name="ids">Ids of the members</param>
        public WebDataActionResult AnonymizeUsers(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                foreach (var memberId in GetMemberIds(id))
                    membersPlugin.DeleteMember(memberId);
            }

            return new WebDataActionResult("Hotovo!");
        }

        /// <summary>
        /// Delete the user after the member is deleted by user. The user data gets anonymized.
        /// </summary>
        /// <param name="memberId">Member id to delete.</param>
        public abstract void DeleteUserByMemberId(int memberId);

        /// <summary>
        /// Return if the service is student or not.
        /// </summary>
        protected abstract bool IsStudent { get; }
        protected abstract string ClassIdentifier { get; }
        protected abstract T GetByMemberId(int memberId);
        protected abstract IEnumerable<int> GetMemberIds(int id);
    }
}