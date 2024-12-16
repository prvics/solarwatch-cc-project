using SolarWatch.DTOs;
using SolarWatch.Models;


namespace SolarWatch.Services;

public interface ISolarWatchService
{
    Task<SunsetSunriseModel> GetSunriseSunset(string city, DateTime dateTime);
    Task<CityModel> CreateCity(CityModel cityData);
    Task<CityModel> UpdateCity(int id, UpdateCityDto city);
    Task<CityModel> DeleteCity(int id);
    Task<CityModel> GetCityById(int id);
    Task<SunsetSunriseModel> CreateSunriseSunset(SunsetSunriseModel sunsetSunriseModel);
    Task<SunsetSunriseModel> UpdateSunriseSunset(int id, UpdateSunsetSunriseDto sunriseSunset);
    Task<SunsetSunriseModel> DeleteSunriseSunset(int id);
    Task<SunsetSunriseModel> GetSunriseSunsetById(int id);
}