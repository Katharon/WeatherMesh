namespace RegenbogenRadar.Domain.WeatherStation
{
    using System;

    public class WeatherStation
    {
        Guid Id { get; set; }
        
        bool IsActive { get; set; }

        string Name { get; set; } = string.Empty;

        double Latitude { get; set; }

        double Longitude { get; set; }
    }
}