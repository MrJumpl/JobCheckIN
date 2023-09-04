using System;

namespace Mlok.Web.Sites.JobChIN.Models.StudentModels
{
    public interface IStudy
    {
        string Faculty { get; }
        DateTime From { get; }
        string Specialization { get; }
        DateTime? To { get; }
        string University { get; }
    }
}