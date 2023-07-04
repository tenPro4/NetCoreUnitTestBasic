using System.ComponentModel.DataAnnotations.Schema;

namespace StationMonitorAPI.Models
{
    public class vResultIdentifier
    {
        public long ResultID { get; set; }
        public long ResultIdentifierID { get; set; }
        public int SortOrder { get; set; }
        public string Identifier { get; set; }
    }
}
