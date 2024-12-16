using System.Net;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.DTOs;
using SolarWatch.Models;


namespace SolarWatch.Services;

public class SolarWatchService : ISolarWatchService
{
    private readonly SolarWatchContext _context;
    private readonly IJsonProcessor _jsonProcessor;
    

    public SolarWatchService(IJsonProcessor jsonProcessor, SolarWatchContext solarWatchContext)
    {
        _jsonProcessor = jsonProcessor;
        _context = solarWatchContext;
    }
    
    public async Task<SunsetSunriseModel> GetSunriseSunset(string city, DateTime dateTime)
    {
        var cityData = await _context.Cities.FirstOrDefaultAsync(c => c.City.ToLower() == city.ToLower());

        if (cityData == null)
        {
            cityData = await GetCity(city);
            _context.Cities.Add(cityData);
            await _context.SaveChangesAsync();
        }
        
        var formattedDate = dateTime.ToString("yyyy-MM-dd");
        var sunriseSunset = await _context.SunriseSunsets
            .FirstOrDefaultAsync(s => s.CityId == cityData.CityId && s.Date == formattedDate);

        if (sunriseSunset == null)
        {
            sunriseSunset = await GetSunriseSunsetFromApi(cityData, dateTime);
            _context.SunriseSunsets.Add(sunriseSunset);
            await _context.SaveChangesAsync();
        }

        return sunriseSunset;
        
    }

    private async Task<CityModel> GetCity(string city)
    {
        var apiKey = "01dd8942d1a10579fb79c096f5dbe9a6";

        var url =
            $"http://api.openweathermap.org/geo/1.0/direct?q={city}&appid={apiKey}";

        using var client = new HttpClient();
        var data = await client.GetStringAsync(url);

        return  _jsonProcessor.CityProcess(data);
    }

    private async Task<SunsetSunriseModel> GetSunriseSunsetFromApi(CityModel cityData, DateTime dateTime)
    {
        var lat = cityData.Lat;
        var lon = cityData.Lon;
        var formattedDate = dateTime.ToString("yyyy-MM-dd");
        
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={formattedDate}";

        using var client = new HttpClient();
        var data = await client.GetStringAsync(url);

        return  _jsonProcessor.Process(data, cityData, formattedDate);
    }

    public async Task<CityModel> CreateCity(CityModel cityData)
    {
        _context.Cities.Add(cityData);
        await _context.SaveChangesAsync();
        return cityData;
    }

    public async Task<CityModel> UpdateCity(int id, UpdateCityDto city)
    {
        var cityFound = await _context.Cities.FirstOrDefaultAsync(c => c.CityId == id);
    
        if (cityFound == null)
        {
            return null;
        }

        cityFound.CityId = city.CityId;
        cityFound.City = city.City;
        cityFound.Lat = city.Lat;
        cityFound.Lon = city.Lon;
        cityFound.Country = city.Country;
        cityFound.State = city.State;
        
        await _context.SaveChangesAsync();
        

        return cityFound;
    }

    public async Task<CityModel> DeleteCity(int id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city == null)
        {
            return null;
        }

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();
        return city;
    }

    public async Task<CityModel> GetCityById(int id)
    {
        var city = await _context.Cities.FirstOrDefaultAsync(c => c.CityId == id);
        return city;
    }

    public async Task<SunsetSunriseModel> CreateSunriseSunset(SunsetSunriseModel sunsetSunriseModel)
    {
        _context.SunriseSunsets.Add(sunsetSunriseModel);
        await _context.SaveChangesAsync();

        return sunsetSunriseModel;
    }

    public async Task<SunsetSunriseModel> UpdateSunriseSunset(int id, UpdateSunsetSunriseDto sunriseSunset)
    {
        var sunriseSunsetFound = await _context.SunriseSunsets.FirstOrDefaultAsync(s => s.Id == id);

        if (sunriseSunsetFound == null)
        {
            return null;
        }

        sunriseSunsetFound.Id = sunriseSunset.Id;
        sunriseSunsetFound.City = sunriseSunset.City;
        sunriseSunsetFound.Sunrise = sunriseSunset.Sunrise;
        sunriseSunsetFound.Sunset = sunriseSunset.Sunset;
        sunriseSunsetFound.Date = sunriseSunset.Date;
        sunriseSunsetFound.CityId = sunriseSunset.CityId;
        
        await _context.SaveChangesAsync();
        return sunriseSunsetFound;
    }

    public async Task<SunsetSunriseModel> DeleteSunriseSunset(int id)
    {
        var sunriseSunset = await _context.SunriseSunsets.FindAsync(id);
        if (sunriseSunset == null)
        {
            return null;
        }
        
        _context.SunriseSunsets.Remove(sunriseSunset);
        await _context.SaveChangesAsync();
        
        return sunriseSunset;
    }

    public async Task<SunsetSunriseModel> GetSunriseSunsetById(int id)
    {
        var sunriseSunset = await _context.SunriseSunsets.FindAsync(id);
        if (sunriseSunset == null)
        {
            return null;
        }

        return sunriseSunset;
    }
}