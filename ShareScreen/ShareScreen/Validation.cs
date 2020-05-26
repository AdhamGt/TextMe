using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShareScreen
{
   static class Validation
    {
        public static bool ValidatePassswordConstrains( string password)
        {
            if (!password.Contains(" ")&& !password.Contains("_") && Regex.IsMatch(password, "^[a-zA-Z0-9]+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool ValidatePinConstrains(string password)
        {
            if (!password.Contains(" ") && !password.Contains("_") && Regex.IsMatch(password, "^[0-9]+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool ValidateUsernameConstrains(string username)
        {
            if (username != "" && !username.Contains(" ") && Regex.IsMatch(username, "^[a-zA-Z0-9_]+$") && !Regex.IsMatch(username[0].ToString(), "^[0-9_]+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
