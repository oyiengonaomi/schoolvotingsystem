using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Votingsystem.Data;
using Votingsystem.Models;

namespace Votingsystem.Controllers
{
    [Authorize(Roles = "Election Commissioner")]
    public class ElectionCommissionerController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        public ElectionCommissionerController(ApplicationDbContext context)
        {
            // 3. We take the 'context' delivered by the system and put it in our storage
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myPolls = await _context.Polls
                .Where(p => p.CreatedByUserId == userId)
                .Include(p => p.Candidates)
                .OrderByDescending(p => p.StartDate)
                .ToListAsync();

            return View(myPolls);
        }

        // GET: ElectionCommissioner/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var poll = await _context.Polls.FindAsync(id);
            if (poll == null) return NotFound();

            // Ensure only the creator can edit it
            if (poll.CreatedByUserId != User.FindFirstValue(ClaimTypes.NameIdentifier)) return Forbid();

            return View(poll);
        }

        // POST: ElectionCommissioner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Poll updatedPoll)
        {
            if (id != updatedPoll.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // We only want to update specific fields to avoid overwriting the CreatorId
                    var existingPoll = await _context.Polls.FindAsync(id);
                    if (existingPoll == null) return NotFound();

                    existingPoll.Title = updatedPoll.Title;
                    existingPoll.Description = updatedPoll.Description;
                    existingPoll.StartDate = updatedPoll.StartDate;
                    existingPoll.EndDate = updatedPoll.EndDate;
                    existingPoll.IsActive = updatedPoll.IsActive;

                    _context.Update(existingPoll);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Poll updated successfully!";
                    return RedirectToAction(nameof(Dashboard));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Unable to save changes. The poll was modified by another process.");
                }
            }
            return View(updatedPoll);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Pre-set some default dates to make the form easier to fill
            var poll = new Poll
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7)
            };
            return View(poll);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Poll poll)
        {
            // 1. Get the current Commissioner's ID from the logged-in user
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 2. Assign it to the model
            poll.CreatedByUserId = currentUserId;

            // 3. Validation: Ensure dates make sense
            if (poll.EndDate <= poll.StartDate)
            {
                ModelState.AddModelError("EndDate", "The election must end after it starts!");
            }

            if (ModelState.IsValid)
            {
                // 4. Assign the current Commissioner's ID
                poll.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // 5. Save to Database
                _context.Polls.Add(poll);
                await _context.SaveChangesAsync();

                // 6. Success! Redirect to add candidates for THIS specific poll
                TempData["Success"] = "Poll created! Now, let's add some candidates.";
                return RedirectToAction("AddCandidates", new { id = poll.Id });
            }

            // If we reach here, something went wrong (validation failed)
            return View(poll);
        }

        // GET: ElectionCommissioner/AddCandidates/5
        public async Task<IActionResult> AddCandidates(int id)
        {
            var poll = await _context.Polls
                .Include(p => p.Candidates)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (poll == null) return NotFound();

            // We pass the poll to the view so we know WHICH poll we are adding people to
            return View(poll);
        }


        // POST: ElectionCommissioner/AddCandidates
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCandidateToPoll(int pollId, string name, string manifesto)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var candidate = new Candidate
                {
                    Name = name,
                    Manifesto = manifesto,
                    PollId = pollId,
                    VoteCount = 0 // Starts at zero
                };

                _context.Candidates.Add(candidate);
                await _context.SaveChangesAsync();
            }

            // Redirect back to the same page to allow adding another candidate
            return RedirectToAction("AddCandidates", new { id = pollId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCandidate(int candidateId, int pollId)
        {
            var candidate = await _context.Candidates.FindAsync(candidateId);

            if (candidate != null)
            {
                _context.Candidates.Remove(candidate);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Candidate removed successfully.";
            }

            // Redirect back to the AddCandidates page for this specific poll
            return RedirectToAction("AddCandidates", new { id = pollId });
        }
    }

}
