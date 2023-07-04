using System.ComponentModel.DataAnnotations.Schema;

namespace StationMonitorAPI.Models
{
    public class Program
    {
        public long ID { get; set; }
        public long? OriginalProgramVersionID { get; set; }
        [ForeignKey(nameof(OriginalProgramVersionID))]
        public Program? OriginalProgramVersion { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public DateTime ProgramDateTime { get; set; }
        public string? Comment { get; set; }
        public bool HasParameters { get; set; } = false;
        public long ProgramTypeID { get; set; }
        [ForeignKey(nameof(ProgramTypeID))]
        public ProgramType ProgramType { get; set; }
        public string? ProgramIdentifier { get; set; }
        public string? ProgramVersion { get; set; }
        public string? ChangedBy { get; set; }
        public bool? IsHidden { get; set; }
    }
}
