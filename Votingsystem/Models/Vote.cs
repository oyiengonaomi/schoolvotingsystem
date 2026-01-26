using System.ComponentModel.DataAnnotations;

namespace Votingsystem.Models
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string VoterId { get; set; } = string.Empty; // The ID of the student

        [Required]
        public int PollId { get; set; } // Which election this vote is for

        [Required]
        public int CandidateId { get; set; } // Who they picked

        public DateTime VoteDate { get; set; } = DateTime.Now;
    }
}
