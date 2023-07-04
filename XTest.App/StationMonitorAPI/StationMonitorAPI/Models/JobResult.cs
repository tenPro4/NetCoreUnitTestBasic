using System.ComponentModel.DataAnnotations.Schema;

namespace StationMonitorAPI.Models
{
    public class JobResult
    {
        public long ID { get; set; }
        public long ProgramID { get; set; }
        [ForeignKey(nameof(ProgramID))]
        public Program Program { get; set; }
        public long SequenceNumber { get; set; }
        public DateTime JobStartTime { get; set; }
        public DateTime? JobEndTime { get; set; }
        public long JobResultStatusTypeID { get; set; }
        [ForeignKey(nameof(JobResultStatusTypeID))]
        public JobResultStatusType Status { get; set; }
        public long? UnitID { get; set; }
        [ForeignKey(nameof(UnitID))]
        public Unit? Unit { get; set; }
        public string JobResultIdentifier { get; set; }

    }
}
