using Database.Test.Infrastructure;
using XTest.Database.Models;

namespace Database.Test
{
    public class AppDbContextTests
    {
        [Fact]
        public void SavesCounterToDatabase()
        {
            var counter = new Counter { Name = "New Counter" };
            using (var ctx = DbContextFactory.Create(nameof(SavesCounterToDatabase)))
            {
                ctx.Counters.Add(counter);
                ctx.SaveChanges();
            }

            using(var ctx = DbContextFactory.Create(nameof(SavesCounterToDatabase)))
            {
                var savedCounter = ctx.Counters.Single();
                Assert.Equal(counter.Name, savedCounter.Name);
            }
        }

    }
}