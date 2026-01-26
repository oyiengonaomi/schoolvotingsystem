using System.ComponentModel.DataAnnotations;

namespace Votingsystem.Models
{
    public class Poll
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Foreign Key for the Commissioner who created it
        public string CreatedByUserId { get; set; } = string.Empty;

        public ApplicationUser? CreatedBy { get; set; }

        // Navigation property: One poll has many candidates
        public ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
    }
}
