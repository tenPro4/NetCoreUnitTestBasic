using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using XTest.Database.Models;

namespace XTest.Database
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Vote> Votes { get; set; }
        public DbSet<Counter> Counters { get; set; }
        public DbSet<VotingPoll> VotingPolls { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Counter>().Property<int>("Id");
            modelBuilder.Entity<Counter>().Ignore(x => x.Count);

            modelBuilder.Entity<VotingPoll>().Property<int>("Id");

            modelBuilder.Entity<Vote>().Property<int>("Id");
        }
    }
}