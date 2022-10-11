using Microsoft.AspNetCore.Mvc;
using XTest.Database.Models;
using XTesting.Services;

namespace XTest.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IVotingPollFactory _pollFactory;

        public HomeController(IVotingPollFactory pollFactory)
        {
            _pollFactory = pollFactory;
        }

        [HttpPost]
        public VotingPoll Create(VotingPollFactory.Request request)
        {
            return _pollFactory.Create(request);
        }
    }
}
