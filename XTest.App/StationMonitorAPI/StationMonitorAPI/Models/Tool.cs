using System.ComponentModel.DataAnnotations.Schema;

namespace StationMonitorAPI.Models
{
    public class Tool
    {
        public long ID { get; set; }
        public string Identifier { get; set; }
        public string? CustomField { get; set; }
        public string? ModelType { get; set; }
        public string? SerialNumber { get; set; }
        public int? TighteningCount { get; set; }
        public DateTime? ServiceDate { get; set; }
        public DateTime? CalibrationDate { get; set; }
        public DateTime? LatestUpdatedDate { get; set; }
        public Int16? ToolTypeID { get; set; }
        [ForeignKey(nameof(ToolTypeID))]
        public ToolType? ToolType { get; set; }
        public bool? IsManual { get; set; }
    }
}
