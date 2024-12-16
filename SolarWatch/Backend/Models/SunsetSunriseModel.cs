namespace SolarWatch.Models;

public class SunsetSunriseModel
{
    public int Id { get; set; }
    public string City { get; set; }
    public string Date { get; set; }
    public string? Sunrise { get; set; }
    public string? Sunset { get; set; }
    public int CityId { get; set; }
    public CityModel? CityModel { get; set; }
}