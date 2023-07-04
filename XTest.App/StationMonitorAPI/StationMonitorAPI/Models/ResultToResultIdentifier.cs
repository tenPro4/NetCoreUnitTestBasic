using System.ComponentModel.DataAnnotations.Schema;

namespace StationMonitorAPI.Models
{
    public class ResultToResultIdentifier
    {
        public long ResultID { get; set; }
        [ForeignKey(nameof(ResultID))]
        public Result Result { get; set; }
        public long ResultIdentifierID { get; set; }
        [ForeignKey(nameof(ResultIdentifierID))]
        public ResultIdentifier ResultIdentifier { get; set; }
        public int SortOrder { get; set; }
    }
}
