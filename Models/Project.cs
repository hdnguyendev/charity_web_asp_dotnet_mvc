using System.ComponentModel.DataAnnotations;

namespace DonationsWeb.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Desciption { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int TotalAmount { get; set; }
        public int CurrentAmount { get; set; }
        public bool Status { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Donation>? Donations { get; set; }
        // FK
        [Display(Name = "User")]
        public int UserId { get; set; }
        public User? User { get; set; }
         
    }
}
