using System.ComponentModel.DataAnnotations.Schema;

namespace StationMonitorAPI.Models
{
    public class ResultIdentifier
    {
        public long ID { get; set; }
        public string Identifier { get; set; }
        public long? ResultIdentifierTypeID { get; set; }
        [ForeignKey(nameof(ResultIdentifierTypeID))]
        public ResultIdentifierType? IdentifierType { get; set; }
    }
}
