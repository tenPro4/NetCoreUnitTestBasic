using System.ComponentModel.DataAnnotations.Schema;

namespace StationMonitorAPI.Models
{
    public class ResultTightening
    {
        public long ResultID { get; set; }
        [ForeignKey(nameof(ResultID))]
        public Result Result { get; set; }
        public double? FinalAngle { get; set; }
        public double? FinalTorque { get; set; }
        public double? RundownAngle { get; set; }
        public long? FinalAngleStatusID { get; set; }
        [ForeignKey(nameof(FinalAngleStatusID))]
        public ResultStatusType? FinalAngleStatus { get; set; }
        public long? FinalTorqueStatusID { get; set; }
        [ForeignKey(nameof(FinalTorqueStatusID))]
        public ResultStatusType? FinalTorqueStatus { get; set; }
        public long? RundownAngleStatusID { get; set; }
        [ForeignKey(nameof(RundownAngleStatusID))]
        public ResultStatusType? FinalRunDownStatus { get; set; }
    }
}
