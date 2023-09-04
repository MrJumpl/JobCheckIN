using System.IO;

namespace Mlok.Web.Sites.JobChIN.Models
{
    public interface ICvModel
    {
        /// <summary>
        /// Stream of the student cv in given format.
        /// </summary>
        MemoryStream Content { get; }
        /// <summary>
        /// File extension of the cv.
        /// </summary>
        string FileExt { get; }
        /// <summary>
        /// Mime type that corresponds to cv format.
        /// </summary>
        string MimeType { get; }
    }
}