using StationMonitorAPI.Configurations.Constants;

namespace StationMonitorAPI.DTOs
{
    public class SearchResultModel
    {
        public string SearchValue { get; set; }
        public bool Latest { get; set; } = true;
    }
}
