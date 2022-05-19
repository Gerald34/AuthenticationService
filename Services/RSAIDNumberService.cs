using System.Text.RegularExpressions;

namespace AuthenticationService.Services
{
    /// <summary>
    /// RSA ID number service
    /// Format validation
    /// Length check
    /// </summary>
    /// <author>Gerald Mathabela</author>
    /// <organisation>Investec Bank</organisation>
    public class RSAIDNumberService
    {
        /// <summary>
        /// RSA ID number format regex
        /// </summary>
        private const string _IdExpression = @"(?<Year>[0-9][0-9])(?<Month>([0][1-9])|([1][0-2]))(?<Day>([0][1-9])|([1-2][0-9])|([3][0-1])|([3][0-1]))(?<Gender>[0-9])(?<Series>[0-9]{3})(?<Citizenship>[0-9])(?<Uniform>[0-9])(?<Control>[0-9])";

        /// <summary>
        /// Valid RSA ID number length
        /// </summary>
        private const int _ValidLength = 13;

        /// <summary>
        /// Get user ID RSA ID number and validate length
        /// </summary>
        /// <param name="rSAIDNumber"></param>
        /// <returns>Boolean</returns>
        public bool ValidRSAID(string userIDNumber)
        {
            if (userIDNumber.Length != _ValidLength) return false;
            return _ValidateID(userIDNumber);
        }

        /// <summary>
        /// Validate ID number as per standards in  South Africa
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns>Boolean</returns>
        private bool _ValidateID(string idNumber)
        {
            return (new Regex(_IdExpression, RegexOptions.IgnoreCase).Match(idNumber).Success) ? true : false;
        }
    }
}