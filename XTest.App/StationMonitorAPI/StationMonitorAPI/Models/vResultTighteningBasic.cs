using System.ComponentModel.DataAnnotations.Schema;

namespace StationMonitorAPI.Models
{
    public class vResultTighteningBasic
    {
        public long ResultID { get; set; }
        public string ProgramName { get; set; }
        public string UnitName { get; set; }
        public DateTime ResultDateTime { get; set; }
        public string OverallStatus { get; set; }
        public string TorqueStatus { get; set; }
        public string AngleStatus { get; set; }
        public double? FinalAngle { get; set; }
        public double? FinalTorque { get; set; }
        public long ResultSequenceNumber { get; set; }
        public DateTime ResultInsertDateTime { get; set; }
        public long ProgramID { get; set; }
        public long UnitID { get; set; }
        public long PositionID { get; set; }
        public long ResultStatusTypeID { get; set; }
        public double? RundownAngle { get; set; }
        public string RundownAngleStatus { get; set; }
    }
}
