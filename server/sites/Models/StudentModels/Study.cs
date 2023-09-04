using Mlok.Core.Utils;
using System;
using System.Collections.Generic;

namespace Mlok.Web.Sites.JobChIN.Models.StudentModels
{
    public class Study : IStudy
    {
        public string Degree { get; set; }
        public string Programme { get; set; }
        public IEnumerable<string> Fields { get; set; }
        public string Faculty { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public string Specialization => Programme;
        public string University => this.Localize("Masarykova univerzita", "Masaryk University");
    }
}