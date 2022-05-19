namespace AuthenticationService.Requests
{
    public class StatusRequest
    {
        public Guid userID { get; set; }
        public string reason { get; set; }
    }
}