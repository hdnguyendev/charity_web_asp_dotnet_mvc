using System.ComponentModel.DataAnnotations.Schema;

namespace DonationsWeb.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int Rating { get; set; } 
        public string Content { get; set; }
        public string Date { get; set; }
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
