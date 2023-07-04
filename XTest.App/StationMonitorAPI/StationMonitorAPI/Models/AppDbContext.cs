using Microsoft.EntityFrameworkCore;

namespace StationMonitorAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet<JobResult> JobResult { get; set; }
        public DbSet<JobResultStatusType> JobResultStatusType { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Program> Program { get; set; }
        public DbSet<ProgramType> ProgramType { get; set; }
        public DbSet<Result> Result { get; set; }
        public DbSet<ResultStatusType> ResultStatusType { get; set; }
        public DbSet<ResultTightening> ResultTightening { get; set; }
        public DbSet<SystemType> SystemType { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<MappingObject> MappingObject { get; set; }
        public DbSet<Tool> Tool { get; set; }
        public DbSet<ToolType> ToolType { get; set; }
        public DbSet<ResultToTool> ResultToTool { get; set; }
        public DbSet<WebSettings> WebSettings { get; set; }
        public DbSet<ResultIdentifier> ResultIdentifier { get; set; }
        public DbSet<ResultIdentifierType> ResultIdentifierType { get; set; }
        public DbSet<ResultToResultIdentifier> ResultToResultIdentifier { get; set; }
        public DbSet<vResultIdentifier> vResultIdentifier { get; set; }
        public DbSet<vResultTighteningBasic> vResultTighteningBasic { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ResultTightening>()
                .HasKey(x => x.ResultID);

            modelBuilder.Entity<ResultToTool>()
                .HasKey(x => new { x.ResultID,x.ToolID});

            modelBuilder.Entity<ResultToResultIdentifier>()
               .HasKey(x => new { x.ResultID, x.ResultIdentifierID });

            modelBuilder.Entity<vResultIdentifier>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vResultIdentifier");
            });

            modelBuilder.Entity<vResultTighteningBasic>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vResultTighteningBasic");
            });
        }
    }
}
