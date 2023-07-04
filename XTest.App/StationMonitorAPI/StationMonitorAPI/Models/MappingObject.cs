using System.ComponentModel.DataAnnotations.Schema;

namespace StationMonitorAPI.Models
{
    public class MappingObject
    {
        public Guid Id { get; set; }
        public long? UnitID { get; set; }
        [ForeignKey(nameof(UnitID))]
        public Unit? Unit { get; set; }
        public int? PositionX { get; set; } = 0;
        public int? PositionY { get; set; } = 0;
    }
}
