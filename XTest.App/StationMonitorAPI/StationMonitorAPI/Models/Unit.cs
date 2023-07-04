using System.ComponentModel.DataAnnotations.Schema;

namespace StationMonitorAPI.Models
{
    public class Unit
    {
        public long ID { get; set; }
        public long SystemTypeId { get; set; }
        [ForeignKey(nameof(SystemTypeId))]
        public SystemType SystemType { get; set; }
        public string? Name { get; set; }
        public string Identifier { get; set; }
        public long? OriginalUnitVersionID { get; set; }
        [ForeignKey(nameof(OriginalUnitVersionID))]
        public Unit? OriginalUnitVersion { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public long? MasterUnitID { get; set; }
        [ForeignKey(nameof(MasterUnitID))]
        public Unit? MasterUnit { get; set; }
        public bool IsMaster { get; set; } = true;
        public string? Comment { get; set; }
        public string? UnitIdentifier { get; set; }
        public string? IPAddress { get; set; }

    }
}
