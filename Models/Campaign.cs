namespace DonationsWeb.Models
{
    public class Campaign
    {
        public int CampaignId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public float Goal { get; set; }
        public float CurrentAmount { get; set; }

        // Save a thumbnail image
        public string? Image { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }


        public ICollection<Donation>? Donations { get; set; }
    }
}
