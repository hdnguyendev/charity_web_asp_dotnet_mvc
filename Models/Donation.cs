using System.ComponentModel.DataAnnotations.Schema;

namespace DonationsWeb.Models
{
    public class Donation
    {
        public int DonationId { get; set; }
        public int Amount { get; set; }
        public string Date { get; set; }
        public string Note { get; set; }
        public bool Status { get; set; }

        // FK
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

    }
}
