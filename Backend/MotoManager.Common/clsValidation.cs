using System.Text.RegularExpressions;

namespace MotoManager.Common
{


    public class clsValidatoin
    {


        public static bool ValidateEmail(string emailAddress)
        {
            var pattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)+$";

            var regex = new Regex(pattern);

            return regex.IsMatch(emailAddress);
        }

        public static bool ValidateInteger(string Number)
        {
            var pattern = @"^[0-9]*$";

            var regex = new Regex(pattern);

            return regex.IsMatch(Number);
        }

        public static bool ValidateFloat(string Number)
        {
            var pattern = @"^[0-9]*(?:\.[0-9]*)?$";
            var regex = new Regex(pattern);
            return regex.IsMatch(Number);
        }

        public static bool IsNumber(string Number)
        {
            return (ValidateInteger(Number) || ValidateFloat(Number));
        }

        public static bool ValidatePhoneNumber(string Number)
        {
            var pattern = @"^[0-9]{10,}$";
            var regex = new Regex(pattern);

            return regex.IsMatch(Number);
        }

        public static bool ValidatePassword(string Password)
        {
            var pattern = @"^(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{8,}$";
            var regex = new Regex(pattern);

            return regex.IsMatch(Password);
        }

        public static bool ValidateUsername(string Username)
        {
            var pattern = @"^[a-zA-Z0-9_]{3,20}$";
            var regex = new Regex(pattern);

            return regex.IsMatch(Username);
        }
    }


}
