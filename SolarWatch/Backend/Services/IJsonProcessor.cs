using SolarWatch.Models;

namespace SolarWatch.Services;

public interface IJsonProcessor
{
    SunsetSunriseModel Process(string data, CityModel cityModel, string date);
    CityModel CityProcess(string data);
}