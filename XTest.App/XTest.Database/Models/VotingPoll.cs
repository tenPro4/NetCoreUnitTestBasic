using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTest.Database.Models
{
    public class VotingPoll
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Counter> Counters { get; set; } = new List<Counter>();
    }
}
