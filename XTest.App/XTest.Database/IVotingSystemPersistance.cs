using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTest.Database.Models;

namespace XTest.Database
{
    public interface IVotingSystemPersistance
    {
        ICollection<VotingPoll> GetPolls();
        void SaveVotingPoll(VotingPoll votingPoll);
        void SaveVote(Vote vote);
        bool VoteExists(Vote vote);
        VotingPoll GetPoll(int pollId);
    }
}
