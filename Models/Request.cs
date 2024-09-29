namespace DonationsWeb.Models
{
    public class Request
    {
        public int RequestId { get; set; }
        public string RequesterName { get; set; }
        public string Email { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public string RequestMessage { get; set; }
    }
}
