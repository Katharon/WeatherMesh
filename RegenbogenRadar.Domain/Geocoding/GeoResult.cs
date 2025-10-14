namespace RegenbogenRadar.Domain.Geocoding
{
    public class GeoResult
    {
        public string Name { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
