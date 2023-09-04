using Newtonsoft.Json;

namespace Mlok.Web.Sites.JobChIN.Models.Dtos
{
    public class CompanyGeneralInfoDto
    {
        [JsonProperty("ico")]
        public string ICO { get; set; }
        [JsonProperty("dic")]
        public string DIC { get; set; }
        public string CompanyName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
    }
}