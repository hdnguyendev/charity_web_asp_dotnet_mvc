using System.ComponentModel.DataAnnotations.Schema;

namespace DonationsWeb.Models
{
    public class Donation
    {
        public int DonationId { get; set; }
        public int UserId { get; set; }
        public int CampaignId { get; set; }
        public float Amount { get; set; }
        public DateTime DonationDate { get; set; } = DateTime.Now;
        public string Message { get; set; }
        public User? User { get; set; }
        public Campaign? Campaign { get; set; }


    }
}
