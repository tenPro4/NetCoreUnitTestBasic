using Database.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTesting.Services
{
    public class PollStatistics
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<CounterStatistics> Counters { get; set; }
    }
}
