using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTest.Database.Models;

namespace XTesting.Services
{
    public class VotingPollFactory : IVotingPollFactory
    {
        public class Request
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string[] Names { get; set; }
        }

        public VotingPoll Create(Request request)
        {
            if (request.Names.Length < 2) throw new ArgumentException();

            return new VotingPoll
            {
                Title = request.Title,
                Description = request.Description,
                Counters = request.Names.Select(name => new Counter { Name = name }).ToList()
            };
        }
    }
}
