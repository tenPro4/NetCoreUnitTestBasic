using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using XTest.Database;
using XTest.Database.Models;
using XTesting.Services;
using static XTesting.Services.VotingPollFactory;

namespace XTest.App.Controllers
{
    [ApiController]
    [Route("api/polls")]
    public class PollController : ControllerBase
    {
        private readonly IVotingPollFactory _pollFactory;
        private readonly IVotingSystemPersistance _votePersistance;
        private readonly ICounterManager _counterManager;
        private readonly IWebHostEnvironment _env;
        private readonly FileSettings _settings;
        public PollController(
            IVotingPollFactory pollFactory, 
            IVotingSystemPersistance votePersistance, 
            ICounterManager counterManager,
            IWebHostEnvironment env,
            IOptionsMonitor<FileSettings> optionsMonitor)
        {
            _pollFactory = pollFactory;
            _votePersistance = votePersistance;
            _counterManager = counterManager;
            _env = env;
            _settings = optionsMonitor.CurrentValue;
        }

        [HttpPost("vote")]
        [Authorize]
        public IActionResult CreateVote(Vote vote)
        {
            if (!_votePersistance.VoteExists(vote))
            {
                _votePersistance.SaveVote(vote);
                return Ok(vote);
            }

            return BadRequest();
        }

        [HttpPost("poll")]
        [Authorize(Policy = "Admin")]
        public IActionResult CreatePoll(Request request)
        {
            var poll = _pollFactory.Create(request);
            _votePersistance.SaveVotingPoll(poll);

            return Ok(request);
        }

        [HttpGet]
        public IActionResult GetPolls()
        {
            var polls = _votePersistance.GetPolls();

            return Ok(polls);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Privileged")]
        public IActionResult GetPollById(int id)
        {
            var poll = _votePersistance.GetPoll(id);

            var statistics = _counterManager.GetStatistics(poll.Counters);

            _counterManager.ResolveExcess(statistics);

            var result = new PollStatistics
            {
                Title = poll.Title,
                Description = poll.Description,
                Counters = statistics
            };

            return Ok(result);
        }

        [HttpPost("upFile")]
        public async Task<IActionResult> SaveFile([FromForm] IFormFile file)
        {
            var savePath = Path.Combine(_env.WebRootPath, _settings.Path);
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            var fileSavePath = Path.Combine(savePath, file.FileName);
            await using var fileStream = System.IO.File.Create(fileSavePath);
            await file.CopyToAsync(fileStream);
            return Ok();
        }
    }
}