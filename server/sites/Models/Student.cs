using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Models.StudentModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Web.PublishedCache;

namespace Mlok.Web.Sites.JobChIN.Models
{
    [ModelEditor(typeof(StudentWebDataFormatter))]
    public class Student : IUser, IUserMedia, IAnonymizable
    {
        public int StudentId { get; set; }
        [JsonIgnore]
        public MemberPublishedContent Member { get; set; }
        public int Uco { get; set; }
        public int MediaFolderId { get; set; }
        public bool ProvideContact { get; set; }
        public DateTime? LastTimeUpdatedByStudent { get; set; }
        public int AgreedTo { get; set; }

        public string Interests { get; set; }

        public BasicInfo BasicInfo { get; set; }
        public StudentPhoto Photo { get; set; }
        public WorkExperiences WorkExperiences { get; set; }
        public Studies Studies { get; set; }
        public Skills Skills { get; set; }
        public Contact Contact { get; set; }
        public NotificationSettings NotificationSettings { get; set; }
        public Visibility Visibility { get; set; }

        // automated data from database
        public string FullName { get; set; }
        public string CvFileName { get; set; }
        public DateTime? ShownInterest { get; set; }
        public IEnumerable<Study> MuniStudies { get; set; }
        public MatchCategory Match { get; set; }
        public bool IsRevealedByCompany { get; set; }


        public DateTime? EndAccess()
        {
            if (!MuniStudies?.Any() ?? true)
                return DateTime.MinValue;
            if (MuniStudies.Any(x => !x.To.HasValue))
                return null;
            return MuniStudies.Max(x => x.To.Value).AddMonths(AgreedTo);
        }

        public string EmailForNotiofications()
        {
            if (!string.IsNullOrWhiteSpace(NotificationSettings?.NotificationEmail))
                return NotificationSettings.NotificationEmail;
            if (!string.IsNullOrWhiteSpace(Contact?.PrivateEmail))
                return Contact.PrivateEmail;
            return Member?.Email;
        }

        public int Completeness()
        {
            decimal max = 0;
            int count = 0;
            foreach (var item in GetModelCompleteness())
            {
                count += item.Completeness();
                max += item.FullyCompleted();
            }
            return (int)Math.Round(count / max * 100);
        }

        public void AnonymizeData()
        {
            Uco = 0;
            foreach (var item in GetAnonymizable())
                item.AnonymizeData();
        }

        IEnumerable<ICompleteness> GetModelCompleteness()
        {
            return new ICompleteness[] { BasicInfo, WorkExperiences, Skills, Studies, Contact };
        }

        IEnumerable<IAnonymizable> GetAnonymizable()
        {
            return new IAnonymizable[] { BasicInfo, Photo, WorkExperiences, Skills, Studies, Contact, NotificationSettings, Visibility };
        }

        public class StudentWebDataFormatter : AbstractModelEditor<Student, JobChINModule>
        {
            public StudentWebDataFormatter()
            {
                Setup(cfg =>
                {
                    cfg.SetName("UČO", x => x.Uco.ToString());
                });

                SetupOverview(listviewCfg =>
                {
                    listviewCfg.AddField("Jméno", x => x.FullName);
                    listviewCfg.AddField("Poslední aktualizace", x => x.LastTimeUpdatedByStudent);
                    listviewCfg.AddField("Notifikační e-mail", x => x.NotificationSettings?.NotificationEmail);
                    listviewCfg.AddField("Telefon", x => x.Contact?.Phone);
                });

                SetupModelEditor(detailCfg =>
                {
                    detailCfg.AddTab("Základní informace", tabConfig =>
                    {
                        tabConfig.AddField("Základní informace", x => x.BasicInfo);
                    });
                    detailCfg.AddTab("Pracovní zkušenosti", tabConfig =>
                    {
                        tabConfig.AddField("Pracovní zkušenosti", x => x.WorkExperiences);
                    });
                    detailCfg.AddTab("Dovednosti", tabConfig =>
                    {
                        tabConfig.AddField("Dovednosti", x => x.Skills);
                    });
                    detailCfg.AddTab("Vzdělání", tabConfig =>
                    {
                        tabConfig.AddField("Vzdělání", x => x.Studies);
                    });
                    detailCfg.AddTab("Kontakt", tabConfig =>
                    {
                        tabConfig.AddField("Kontakt", x => x.Contact);
                    });
                    detailCfg.AddTab("Nastavení notifikací", tabConfig =>
                    {
                        tabConfig.AddField("Nastavení notifikací", x => x.NotificationSettings);
                    });
                    detailCfg.AddTab("Sobory", tabConfig =>
                    {
                        tabConfig.AddField("Složka v mediích", x => x.MediaFolderId)
                            .SetDataType(x => x.ReadOnlyMedia());
                    });
                });
            }
        }
    }
}