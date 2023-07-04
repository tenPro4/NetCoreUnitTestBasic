using StationMonitorAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace StationMonitorAPI.DTOs
{
    public class UpdateMappingObjectDto
    {
        public Guid Id { get; set; }
        public long? UnitID { get; set; }
        public long? ProgramID { get; set; }
        public string Type { get; set; }
        public int LeftPos { get; set; }
        public int TopPos { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string? Content { get; set; }
    }
}
