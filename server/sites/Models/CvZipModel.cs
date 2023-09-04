using System.IO;

namespace Mlok.Web.Sites.JobChIN.Models
{
    public class CvZipModel : ICvModel
    {
        public MemoryStream Content { get; }

        public string FileExt => "zip";

        public string MimeType => "application/zip";

        public CvZipModel(MemoryStream stream)
        {
            Content = stream;
        }
    }
}