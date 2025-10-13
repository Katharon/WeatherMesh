namespace RegenbogenRadar.Domain
{
    public class WeatherPayload
    {
        public string Location { get; set; } = string.Empty;
        public string TimeIso { get; set; } = string.Empty;
        public HourlyBrief[] Hourly { get; set; } = [];
    }
}