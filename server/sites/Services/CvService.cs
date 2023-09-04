using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Ionic.Zip;
using MiniSoftware;
using Mlok.Core.Data;
using Mlok.Core.Utils;
using Mlok.Web.Sites.JobChIN.Controllers;
using Mlok.Web.Sites.JobChIN.Models;
using Mlok.Web.Sites.JobChIN.Models.StudentModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.Web;
using UmbracoContrants = Umbraco.Core.Constants;

namespace Mlok.Web.Sites.JobChIN.Services
{
    public class CvService
    {
        private readonly ISettings settings;
        private readonly IMediaService mediaService;

        private readonly StudentPhotoController studentPhotoController;
        private readonly LocalAdministrativeUnitsController localAdministrativeUnitsController;
        private readonly LanguageController languageController;
        private readonly HardSkillController hardSkillController;
        private readonly SoftSkillController softSkillController;


        public CvService(DbScopeProvider scopeProvider, ISettings settings, IMediaService mediaService)
        {
            this.settings = settings;
            this.mediaService = mediaService;

            studentPhotoController = new StudentPhotoController(scopeProvider);
            localAdministrativeUnitsController = new LocalAdministrativeUnitsController(scopeProvider);
            languageController = new LanguageController(scopeProvider);
            hardSkillController = new HardSkillController(scopeProvider);
            softSkillController = new SoftSkillController(scopeProvider);
        }

        /// <summary>
        /// Return students cv in docx format.
        /// </summary>
        /// <param name="student">Student model for cv.</param>
        public ICvModel GetDocxCv(Student student)
        {
            var stream = new MemoryStream();

            // fill in the template
            var templatePath = settings.StudentCvDocxTemplate.GetPropertyValue<string>(UmbracoContrants.Conventions.Media.File);
            MiniWord.SaveAsByTemplate(stream, GetFile(templatePath), GetTempalteValues(student));

            // remove unused template variables
            stream.Seek(0, SeekOrigin.Begin);
            using (var docx = WordprocessingDocument.Open(stream, true))
            {
                var paragraphs = docx.MainDocumentPart.Document.Body.Descendants<Paragraph>();
                foreach (var item in paragraphs)
                {
                    if (item.InnerText.StartsWith("{{"))
                        item.Remove();
                }

                docx.Save();
            }

            stream.Seek(0, SeekOrigin.Begin);

            return new CvDocxModel(stream);
        }

        /// <summary>
        /// Returns the zip of cvs.
        /// </summary>
        public ICvModel GetDocxCvs(IEnumerable<Student> students) => GetCvs(students, GetDocxCv);

        private ICvModel GetCvs(IEnumerable<Student> students, Func<Student, ICvModel> getCvFunc)
        {
            MemoryStream stream = new MemoryStream();

            try
            {
                using (ZipFile zip = new ZipFile() { ParallelDeflateThreshold = -1 })
                {
                    foreach (var student in students)
                    {
                        var model = getCvFunc(student);

                        int i = 0;
                        string fileName = student.CvFileName;
                        string name = $"{fileName}.{model.FileExt}";
                        while (zip.ContainsEntry(name))
                        {
                            name = $"{fileName} ({i}).{model.FileExt}";
                            i++;
                        }

                        zip.AddEntry(name, model.Content);
                    }
                    zip.Save(stream);
                }

                stream.Seek(0, SeekOrigin.Begin);

                return new CvZipModel(stream);
            }
            catch
            {
                if (stream != null)
                    stream.Close();

                throw;
            }
        }

        private Dictionary<string, object> GetTempalteValues(Student student)
        {
            var tmpValues = new Dictionary<string, object>()
            {
                ["FullName"] = student.FullName,
                ["Skill"] =
                    new List<Dictionary<string, object>>()
                    {
                        new Dictionary<string, object>()
                        {
                            {"DisplayValue", "Angličtina - A2"},
                        },
                        new Dictionary<string, object>()
                        {
                            {"DisplayValue", "Němčina - B1"},
                        },
                    },
            };

            var photo = new MiniWordPicture() { Width = 100, Height = 100 };
            if (student.Photo.Photo.HasValue)
            {
                var media = mediaService.GetById(student.Photo.Photo.Value);
                photo.Bytes = GetFile(media.GetValue<string>(UmbracoContrants.Conventions.Media.File));
                photo.Extension = media.GetValue<string>(UmbracoContrants.Conventions.Media.Extension);
            }
            else
            {
                var task = studentPhotoController.GetPhoto(student.Uco);
                photo.Bytes = task.Result;
                photo.Extension = "jpg";
            }
            tmpValues.Add("Photo", photo);

            var contacts = new List<Dictionary<string, object>>();
            if (!student.BasicInfo.WillingToMove)
                contacts.Add(new Dictionary<string, object>() { { "DisplayValue", localAdministrativeUnitsController.GetName(student.BasicInfo.PreferedLocationId) }, });
            if (!student.Contact.Phone.IsNullOrWhiteSpace())
                contacts.Add(new Dictionary<string, object>() { { "DisplayValue", student.Contact.Phone }, });
            if (!student.Contact.PrivateEmail.IsNullOrWhiteSpace())
                contacts.Add(new Dictionary<string, object>() { { "DisplayValue", student.Contact.PrivateEmail }, });
            tmpValues.Add("Contact", contacts);

            tmpValues.Add("Study", GetStudies(student));

            tmpValues.Add("WorkExperience", GetWorkExperiences(student));

            var languages = new List<Dictionary<string, object>>();
            foreach (var lang in languageController.GetDisplayLanguages(student.Skills.Languages))
            {
                languages.Add(new Dictionary<string, object>() { {"DisplayValue", lang.Item2}, });
            }
            tmpValues.Add("Language", languages);

            return tmpValues;
        }

        private byte[] GetFile(string path)
        {
            path = HttpContext.Current.Server.MapPath(path);
            using (var stream = FileSystemUtils.OpenBinaryFile(path))
                return stream.ToByteArray();
        }

        private List<Dictionary<string, object>> GetStudies(Student student)
        {
            var result = new List<Dictionary<string, object>>();
            IEnumerable<IStudy> studies = student.MuniStudies;
            studies = studies.UnionIfNotNull(student.Studies.Others)
                .OrderByDescending(x => x.From)
                .ThenBy(x => x.To);

            foreach (var study in studies)
            {
                result.Add(
                    new Dictionary<string, object>()
                    {
                        {"Date", $"{study.From.ToString("MM/yyyy")} – {ToFormat(study.To)}"},
                        {"Specialization", study.Specialization},
                        {"University", $"{study.Faculty} / {study.University}"},
                    });
            }

            return result;
        }

        private List<Dictionary<string, object>> GetWorkExperiences(Student student)
        {
            var result = new List<Dictionary<string, object>>();
            var workExperiences = student.WorkExperiences.Experiences;
            if (!workExperiences?.Any() ?? true)
                return result;

            workExperiences = workExperiences.OrderByDescending(x => x.From)
                .ThenBy(x => x.To);

            foreach (var workExperience in workExperiences)
            {
                var experience = new Dictionary<string, object>()
                    {
                        {"Date", $"{workExperience.From.ToString("MM/yyyy")} – {ToFormat(workExperience.To)}"},
                        {"CompanyName", workExperience.CompanyName},
                        {"Position", workExperience.Position},
                        {"Description", StrUtils.StripTags(workExperience.Description)},
                    };
                if (!workExperience.ContactPerson.IsNullOrWhiteSpace())
                    experience.Add("ContactPerson", $"Ref.: {workExperience.ContactPerson}, {workExperience.Contact}");
                    
                result.Add(experience);
            }

            return result;
        }

        private string ToFormat(DateTime? to) 
            => to.HasValue
                ? to.Value.ToString("MM/yyyy")
                : this.Localize("současnost", "present");
    }
}