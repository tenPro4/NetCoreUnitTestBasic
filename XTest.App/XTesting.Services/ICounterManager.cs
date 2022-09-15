using Database.Test;
using XTest.Database.Models;

namespace XTesting.Services
{
    public interface ICounterManager
    {
        List<CounterStatistics> GetStatistics(ICollection<Counter> counters);
        void ResolveExcess(List<CounterStatistics> counterStats);
    }
}