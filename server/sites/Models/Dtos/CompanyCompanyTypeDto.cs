using Mlok.Core.Services.OC;
using System;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class CompanyCompanyTypeDto
    {
        public int CompanyCompanyTypeId { get; set; }
        public int? NumberOfWorkPosition { get; set; }
        public int? NumberOfStudentsRevealed { get; set; }
        public bool DatabaseSearch { get; set; }
        public bool CompanyPresentation { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public bool Confirmed { get; set; }
        public bool Paid { get; set; }
        public OrderStateConfig OrderState { get; set; }
    }
}