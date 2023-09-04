namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public interface IJoinController<in TParent, in TParentId, in TChildId>
    {
        /// <summary>
        /// Updates the joind data from the parrent. Must exist the mapping configuration between TParent and IEnumerable of TBDModel
        /// </summary>
        /// <param name="parent"></param>
        void Join(TParent parent);
        /// <summary>
        /// Deletes all joined data by parent id.
        /// </summary>
        void DeleteByParentId(TParentId parentId);
        /// <summary>
        /// Delete all joined data by child id.
        /// </summary>
        void DeleteByChildId(TChildId childId);
        /// <summary>
        /// Check if parent has any joined data.
        /// </summary>
        bool ExistsParentId(TParentId parentId);
        /// <summary>
        /// Check if child has any joined data.
        /// </summary>
        /// <param name="childId"></param>
        /// <returns></returns>
        bool ExistsChildId(TChildId childId);
    }
}