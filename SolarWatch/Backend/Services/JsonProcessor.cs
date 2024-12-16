using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using SolarWatch.Models;

namespace SolarWatch.Services;

public class JsonProcessor : IJsonProcessor
{
    public SunsetSunriseModel Process(string data, CityModel cityModel, string date)
    {
        var json = JsonDocument.Parse(data);
        var results = json.RootElement.GetProperty("results");

        return new SunsetSunriseModel
        {
            City = cityModel.City, 
            Date = date,
            Sunrise = results.GetProperty("sunrise").GetString(),
            Sunset = results.GetProperty("sunset").GetString(),
            CityModel = cityModel
        };
    }

    public CityModel CityProcess(string data)
    {
        var json = JsonDocument.Parse(data);
        var firstElement = json.RootElement[0];
        var cityName = firstElement.GetProperty("name").ToString();
        var cityLat = firstElement.GetProperty("lat").GetDouble();
        var cityLon = firstElement.GetProperty("lon").GetDouble();
        var cityCountry = firstElement.GetProperty("country").ToString();
        string? cityState = null;
        if (firstElement.TryGetProperty("state", out var state))
        {
            cityState = state.ToString();
        }
        

        var cityModel = new CityModel
        {
            City = cityName,
            Lat = cityLat,
            Lon = cityLon,
            State = cityState,
            Country = cityCountry
        };

        return cityModel;
    }
}