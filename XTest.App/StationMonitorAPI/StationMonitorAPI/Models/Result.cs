using System.ComponentModel.DataAnnotations.Schema;

namespace StationMonitorAPI.Models
{
    public class Result
    {
        public long ID { get; set; }
        public long ResultStatusTypeID { get; set; }
        [ForeignKey(nameof(ResultStatusTypeID))]
        public ResultStatusType ResultStatusType { get; set; }
        public long ResultSequenceNumber { get; set; }
        public DateTime ResultDateTime { get; set; }
        public DateTime ResultInsertDateTime { get; set; }
        public long ProgramID { get; set; }
        [ForeignKey(nameof(ProgramID))]
        public Program Program { get; set; }
        public long UnitID { get; set; }
        [ForeignKey(nameof(UnitID))]
        public Unit Unit { get; set; }
        public long PositionID { get; set; }
        [ForeignKey(nameof(PositionID))]
        public Position Position { get; set; }
        public long? JobResultID { get; set; }
        [ForeignKey(nameof(JobResultID))]
        public JobResult? JobResult { get; set;}
    }
}
