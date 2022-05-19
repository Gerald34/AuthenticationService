using System.Security.Cryptography;

namespace AuthenticationService.Utils
{
    public static class PasswordEncryptor
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        public static string EncryptString(string password, int iterations)
        {
            // Create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            // Combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            // Format hash with extra information
            return string.Format("$HuSNwYvW72$V1${0}${1}", iterations, base64Hash);
        }

        public static bool decryptString(string password, string hashedPassword)
        {
            bool dycript = false;
            if (String.IsNullOrEmpty(password) && String.IsNullOrEmpty(hashedPassword)) return false;

            // Check hash
            if (hashedPassword != null && !IsHashSupported(hashedPassword))
                throw new NotSupportedException("The hashtype is not supported");

            // Extract iteration and Base64 string
            var splittedHashString = hashedPassword!.Replace("$HuSNwYvW72$V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            // Get hash bytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            // Get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] == hash[i])
                {
                    dycript = true;
                }
            }

            return dycript;

        }

        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$HuSNwYvW72$V1$");
        }
    }
}