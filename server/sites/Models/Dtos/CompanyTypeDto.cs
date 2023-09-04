namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class CompanyTypeDto
    {
        public int CompanyTypeId { get; set; }
        public string Name { get; set; }
        public int PriceProfit { get; set; }
        public int PriceNonProfit { get; set; }
        public int? NumberOfWorkPosition { get; set; }
        public int? NumberOfStudentsRevealed { get; set; }
        public bool CompanyPresentation { get; set; }
        public bool DatabaseSearch { get; set; }
        public bool Visible { get; set; }
    }
}