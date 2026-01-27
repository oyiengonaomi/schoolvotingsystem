using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Votingsystem.Data;
using Votingsystem.Models;

namespace Votingsystem.Controllers
{
    [Authorize(Roles = "Voter")]
    public class VoterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VoterController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Dashboard()
        {
            // Fetch polls that are marked as 'Active'
            var activePolls = await _context.Polls
                .Where(p => p.IsActive == true)
                .ToListAsync();
            return View(activePolls);
        }

        // GET: Voter/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // 1. Get the ID first!
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Find the poll by ID and "Include" the related candidates
            var poll = await _context.Polls
                .Include(p => p.Candidates)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (poll == null)
            {
                // Check if user already voted
                ViewBag.HasVoted = await _context.Votes.
                    AnyAsync(v => v.VoterId == userId && v.PollId == id);

                return NotFound();
            }
            return View(poll);
        }

        [HttpPost]
        public async Task<IActionResult> Preview(int pollId, int candidateId)
        {
            var poll = await _context.Polls.FirstOrDefaultAsync(p => p.Id == pollId);
            var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Id == candidateId);

            if (poll == null || candidate == null) return NotFound();

            // Pass both to the view so the user can see what they picked
            ViewBag.CandidateName = candidate.Name;
            ViewBag.CandidateId = candidateId;

            return View(poll);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CastVote(int pollId, int candidateId)
        {
            // 1. Get the current Logged-in User's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 2. Security Check: Has this user already voted in THIS poll?
            bool alreadyVoted = await _context.Votes
                .AnyAsync(v => v.VoterId == userId && v.PollId == pollId);

            if (alreadyVoted)
            {
                // You can redirect to an error page or show a message
                TempData["Error"] = "You have already cast your vote for this election.";
                return RedirectToAction("Dashboard");
            }

            // 3. Create the Vote record
            var vote = new Vote
            {
                VoterId = userId,
                PollId = pollId,
                CandidateId = candidateId,
                VoteDate = DateTime.Now
            };

            // 4. Update the Candidate's vote count
            var candidate = await _context.Candidates.FindAsync(candidateId);
            if (candidate != null)
            {
                candidate.VoteCount += 1;
            }

            // 5. Save all changes to the Database
            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thank you! Your vote has been recorded.";
            return RedirectToAction("Success", new {id = pollId});
        }

        public async Task<IActionResult> Success(int id)
        {
            var poll = await _context.Polls
                .Include(p => p.Candidates)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (poll == null) return NotFound();

            return View(poll);
        }
    }
}
