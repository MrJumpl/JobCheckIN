using Mlok.Core.Models;
using Mlok.Core.Models.ApiExceptions;
using Mlok.Web.Sites.JobChIN.Models.Dtos;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mlok.Web.Sites.JobChIN.Services
{
    public class AresService
    {
        const string URL_BASE = "https://wwwinfo.mfcr.cz/cgi-bin/ares/";

        /// <summary>
        /// Check if the given ICO is existing in ARES.
        /// </summary>
        /// <param name="ico">ICO to check.</param>
        public async Task<Result<string>> IcoExists(string ico)
        {
            return await GetResponse(ico, ParseCompanyExists);
        }

        /// <summary>
        /// Returns the info about the company based on the ICO. If ther ICO does not exist the returns result with NotFoundException.
        /// </summary>
        /// <param name="ico">ICO to check.</param>
        public async Task<Result<CompanyGeneralInfoDto>> GetCompanyInfo(string ico)
        {
            return await GetResponse(ico, ParseResponse);
        }

        async Task<Result<T>> GetResponse<T>(string ico, Func<string, string, Result<T>> parseResponse)
        {
            var req = (HttpWebRequest)HttpWebRequest.Create(new Uri($"{URL_BASE}darv_bas.cgi?ico={ico}"));
            req.Method = "GET";
            req.Timeout = req.ReadWriteTimeout = 60000; // 60 s

            try
            {
                using (var res = await req.GetResponseAsync())
                using (var resSteam = res.GetResponseStream())
                using (var sr = new StreamReader(resSteam))
                {
                    string body = sr.ReadToEnd();
                    return parseResponse(ico, body);
                }
            }
            catch (Exception e)
            {
                return new Result<T>(new ExternalServiceException($"Error while invoking async Ares URL '{req.RequestUri.ToString()}'", e));
            }
        }

        Result<T> ICOExists<T>(string ico, string response, out XDocument xml)
        {
            // documentation of shorcuts https://wwwinfo.mfcr.cz/ares/xml_doc/schemas/documentation/zkr_103.txt
            xml = XDocument.Parse(response);

            var dNamespace = xml.Root.Attributes().First(x => x.Name.LocalName == "D").Value;

            var searchResult = xml.Root.Descendants(XName.Get("VH", dNamespace)).FirstOrDefault()?.Descendants(XName.Get("K", dNamespace)).FirstOrDefault()?.Value;
            if (searchResult == null)
                return new Result<T>(new ExternalServiceException($"Ares response does not contain search result code '{response}'"));
            if (searchResult != "1")
                return new Result<T>(new NotFoundException($"Company not found with ICO '{ico}'"));
            return null;
        }

        Result<string> ParseCompanyExists(string ico, string response)
        {
            XDocument xml = null;
            var validationResult = ICOExists<string>(ico, response, out xml);
            if (validationResult != null)
                return validationResult;
            return new Result<string>(string.Empty);
        }

        Result<CompanyGeneralInfoDto> ParseResponse(string ico, string response)
        {
            XDocument xml = null;
            var validationResult = ICOExists<CompanyGeneralInfoDto>(ico, response, out xml);
            if (validationResult != null)
                return validationResult;

            var dNamespace = xml.Root.Attributes().First(x => x.Name.LocalName == "D").Value;

            var searchResult = xml.Root.Descendants(XName.Get("VH", dNamespace)).FirstOrDefault()?.Descendants(XName.Get("K", dNamespace)).FirstOrDefault()?.Value;
            if (searchResult == null)
                return new Result<CompanyGeneralInfoDto>(new ExternalServiceException($"Ares response does not contain search result code '{response}'"));
            if (searchResult != "1")
                return new Result<CompanyGeneralInfoDto>(new NotFoundException($"Company not found with ICO '{ico}'"));

            var basic = xml.Root.Descendants(XName.Get("VBAS", dNamespace)).First();
            var address = basic.Descendants(XName.Get("AA", dNamespace)).First();

            var result = new CompanyGeneralInfoDto();
            result.ICO = ico;
            result.DIC = basic.Descendants(XName.Get("DIC", dNamespace)).FirstOrDefault()?.Value;
            result.CompanyName = basic.Descendants(XName.Get("OF", dNamespace)).First().Value;

            result.Street = address.Descendants(XName.Get("NU", dNamespace)).FirstOrDefault()?.Value 
                ?? address.Descendants(XName.Get("NCO", dNamespace)).FirstOrDefault()?.Value
                ?? string.Empty;
            if (result.Street != string.Empty)
                result.Street += " ";
            result.Street += address.Descendants(XName.Get("CD", dNamespace)).First().Value;
            var number = address.Descendants(XName.Get("CO", dNamespace)).FirstOrDefault()?.Value;
            if (number != null)
                result.Street += $"/{number}";

            result.City = address.Descendants(XName.Get("N", dNamespace)).First().Value;
            result.ZipCode = address.Descendants(XName.Get("PSC", dNamespace)).First().Value;
            int countryId = 0;
            int.TryParse(address.Descendants(XName.Get("KS", dNamespace)).First().Value, out countryId);
            result.CountryId = countryId;

            return new Result<CompanyGeneralInfoDto>(result);            
        }
    }
}