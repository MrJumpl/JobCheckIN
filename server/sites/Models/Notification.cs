using System;

namespace Mlok.Web.Sites.JobChIN.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int Type { get; set; }
        public DateTime Created { get; set; }
        public int MemberId { get; set; }
        public bool Read { get; set; }
        public bool Sent { get; set; }
        public string Params { get; set; }

    }
}