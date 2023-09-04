using Mlok.Web.Sites.JobChIN.Models;

namespace Mlok.Web.Sites.JobChIN.Services
{
    public interface IFileService<in TModel, in TCategory> where TModel : IUserMedia
    {
        /// <summary>
        /// Create media folder for new user. The name of the folder is unique for each user.
        /// </summary>
        int CreateFolder(TModel model);

        /// <summary>
        /// Deletes the media folder. Umrbaco deletes media inside the foldere.
        /// </summary>
        void DeleteFolder(TModel model);

        /// <summary>
        /// Update files of the given category. New files are moved from the temp folder into users media forlder. The files missing in current model are deleted.
        /// </summary>
        /// <param name="category">Category o the file to check.</param>
        /// <param name="current">Current user model.</param>
        /// <param name="old">Old user model.</param>
        void UpdateFiles(TCategory category, TModel current, TModel old);
    }
}