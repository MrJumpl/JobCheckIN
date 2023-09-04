using System;

namespace Mlok.Web.Sites.JobChIN.Models.StudentModels
{
    [Flags]
    public enum FileCategory
    {
        Photo = 1 << 0,
        EducationCertificate = 1 << 2,
        LanguageCertificate = 1 << 3,
    }
}