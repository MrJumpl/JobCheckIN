using PhoneNumbers;

namespace Mlok.Web.Sites.JobChIN.Utils
{
    public static class PhoneNumberUtils
    {
        /// <summary>
        /// Validate the phone number. 
        /// </summary>
        /// <param name="rawNumber">Raw number from input.</param>
        public static bool IsValid(string rawNumber)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            try
            {
                var number = phoneNumberUtil.Parse(rawNumber, "CZ");
                return phoneNumberUtil.IsValidNumber(number);
            }
            catch (NumberParseException)
            {
                return false;
            }
        }

        /// <summary>
        /// Format the phone number to international format with country code. The number must be able to parsed by PhoneNumberUtil.
        /// </summary>
        /// <param name="rawNumber">Raw number from input.</param>
        public static string Format(string rawNumber)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var number = phoneNumberUtil.Parse(rawNumber, "CZ");
            return phoneNumberUtil.Format(number, PhoneNumberFormat.INTERNATIONAL);
        }

    }
}