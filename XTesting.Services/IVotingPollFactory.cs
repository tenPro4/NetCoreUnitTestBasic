using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTest.Database.Models;

namespace XTesting.Services
{
    public interface IVotingPollFactory
    {
        VotingPoll Create(VotingPollFactory.Request request);
    }
}
