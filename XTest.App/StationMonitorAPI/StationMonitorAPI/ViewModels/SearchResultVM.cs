using StationMonitorAPI.Models;

namespace StationMonitorAPI.ViewModels
{
    public class SearchResultVM
    {
        public string Identifier { get; set; }
        public long ResultID { get; set; }
        public DateTime ResultDateTime { get; set; }
        public string OverallStatus { get; set; }
        public string TorqueStatus { get; set; }
        public string AngleStatus { get; set; }
        public string RundownAngleStatus { get; set; }
        public double? FinalAngle { get; set; }
        public double? FinalTorque { get; set; }
        public double? RundownAngle { get; set; }
        public long ResultSequenceNumber { get; set; }
        public string UnitName { get; set; }
        public string ProgramName { get; set; }
        public Unit? Unit { get; set; }
        public Models.Program Program { get; set; }

    }
}
