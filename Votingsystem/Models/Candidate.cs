using System.ComponentModel.DataAnnotations;

namespace Votingsystem.Models
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Manifesto { get; set; } // Their "Why vote for me" text

        // Foreign Key: Links this candidate to a specific Poll
        public int PollId { get; set; }
        public Poll? Poll { get; set; }

        // We will increment this every time someone votes for them
        public int VoteCount { get; set; } = 0;
    }
}
