using System.ComponentModel.DataAnnotations.Schema;

namespace DonationsWeb.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int CampaignId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime CommentDate { get; set; } = DateTime.Now;

        public Campaign? Campaign { get; set; }
        public User? User { get; set; }
    }
}
