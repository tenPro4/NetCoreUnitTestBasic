using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using XTest.Database;
using XTest.Database.Models;
using XTesting.Services;

namespace XTest.UI.Pages
{
    public class PollModel : PageModel
    {
        public PollStatistics Statistics { get; set; }
        public void OnGet(int id, 
            [FromServices] ICounterManager counterManager,
            [FromServices] IVotingSystemPersistance persistance)
        {
            var poll = persistance.GetPoll(id);

            var statistics = counterManager.GetStatistics(poll.Counters);

            counterManager.ResolveExcess(statistics);

            Statistics = new PollStatistics
            {
                Title = poll.Title,
                Description = poll.Description,
                Counters = statistics
            };
        }
        public IActionResult OnPost(int counterId, [FromServices] IVotingSystemPersistance persistance)
        {
            var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var vote = new Vote
            {
                UserId = email,
                CounterId = counterId
            };

            if (!persistance.VoteExists(vote))
            {
                persistance.SaveVote(vote);
            }

            return Redirect(Request.Path.Value);
        }
    }
}
