namespace StationMonitorAPI.Models
{
    public class ResultIdentifierType
    {
        public long ID { get; set; }
        public string ResultIdentifierTypeCode { get; set; }
        public Int32 IsStandard { get; set; }
        public string LanguageConstant { get; set; }
        public string? Description { get; set; }
    }
}
