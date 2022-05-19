namespace AuthenticationService.Config
{
    public class AppSettings
    {
        public string Secret { get; set; } = String.Empty;
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; } = String.Empty;
    }

    public class MailSettings
    {
        public string EmailFrom { get; set; } = string.Empty;
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; } = string.Empty;
        public string SmtpPass { get; set; } = string.Empty;
    }
}

