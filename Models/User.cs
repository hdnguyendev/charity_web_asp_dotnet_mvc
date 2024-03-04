namespace DonationsWeb.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int Role { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Donation>? Donations { get; set; }
        public ICollection<Project>? Projects { get; set; }
    }
}
