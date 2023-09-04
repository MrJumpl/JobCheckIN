using System.IO;

namespace Mlok.Web.Sites.JobChIN.Models
{
    public class CvDocxModel : ICvModel
    {
        public MemoryStream Content { get; }
        public string MimeType => "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public string FileExt => "docx";

        public CvDocxModel(MemoryStream stream)
        {
            Content = stream;
        }
    }
}