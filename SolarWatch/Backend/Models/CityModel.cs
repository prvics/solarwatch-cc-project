namespace SolarWatch.Models;

public class CityModel
{
    public int CityId { get; set; }
    public string City { get; set; }
    public double? Lat { get; set; }
    public double? Lon { get; set; }
    public string Country { get; set; }
    public string? State { get; set; }
    public ICollection<SunsetSunriseModel>? SunriseSunsets { get; set; }
}