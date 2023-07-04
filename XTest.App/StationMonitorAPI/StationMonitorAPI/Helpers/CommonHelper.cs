namespace StationMonitorAPI.Helpers
{
    public static class CommonHelper
    {
        public static string RandomImgName(string prefix = "") =>
   $"img{prefix}_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.jpg";

        public static string RandomFileName(string extension, string prefix = "") =>
   $"{prefix}_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}{extension}";

        public static DateTime ConvertOADateTime(string datetime)
        {
            double d = double.Parse(datetime);
            return DateTime.FromOADate(d);
        }
    }
}
