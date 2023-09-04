using Mlok.Core.Utils;
using Mlok.Modules.WebData;
using Mlok.Web.Sites.JobChIN.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mlok.Web.Sites.JobChIN.Utils
{
    public static class NotificationFrequencyUtils
    {
        public static IEnumerable<EnumerablePickerValue<NotificationFrequency, string>> GetPicker()
            => Enum.GetValues(typeof(NotificationFrequency)).Cast<NotificationFrequency>().Select(y => EnumerablePickerValue.From(y, EnumUtils.GetDisplayName(y)));
    }
}