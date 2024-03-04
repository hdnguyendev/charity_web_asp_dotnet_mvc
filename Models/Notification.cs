namespace DonationsWeb.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
        public bool Status { get; set; }
        // FK
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
