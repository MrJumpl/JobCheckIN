using System;

namespace Mlok.Web.Sites.JobChIN.Models.CompanyModels
{
    [Flags]
    public enum FileCategory
    {
        Logo = 1 << 0,
        BackgroundImage = 1 << 1,
    }
}