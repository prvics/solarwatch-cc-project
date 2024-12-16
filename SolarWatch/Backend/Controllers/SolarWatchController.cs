using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.DTOs;
using SolarWatch.Models;
using SolarWatch.Services;

namespace SolarWatch.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ISolarWatchService _solarWatchService;
    private readonly ILogger<SolarWatchController> _logger;
   
    

    public SolarWatchController(ISolarWatchService solarWatchService, ILogger<SolarWatchController> logger)
    {
        _solarWatchService = solarWatchService;
        _logger = logger;
    }

    [HttpGet(Name = "GetSunriseSunset"), Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> GetSunriseSunset([FromQuery] string city, [FromQuery] DateTime dateTime)
    {
        try
        {
            Debug.WriteLine(this.HttpContext.User.Identity.Name);
            _logger.LogInformation($"City: {city}, Date: {dateTime}");
            var result = await _solarWatchService.GetSunriseSunset(city, dateTime);
            _logger.LogInformation("Response: {@response}", Response);
            _logger.LogInformation($"Service returned: {result}");
            return Ok(new
            {
                Id = result.Id,
                City = result.City,
                Date = result.Date,
                Sunrise = result.Sunrise,
                Sunset = result.Sunset,
                CityModel = new
                {
                    CityId = result.CityModel.CityId,
                    City = result.CityModel.City,
                    Lat = result.CityModel.Lat,
                    Lon = result.CityModel.Lon,
                    Country = result.CityModel.Country,
                    State = result.CityModel.State
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data");
            return NotFound("Unable to retrieve data");
        }
    }
    
    //CRUD
    [HttpPost("cities"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCity([FromBody] CityModel city)
    {
        if (city == null)
        {
            return BadRequest("Invalid city data.");
        }

        await _solarWatchService.CreateCity(city);
        return CreatedAtAction(nameof(GetCityById), new { id = city.CityId }, city);
    }

    [HttpPut("cities/{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCity(int id, [FromBody] UpdateCityDto city)
    {
        if (id != city.CityId)
        {
            return BadRequest("City ID mismatch.");
        }
        
        var updatedCity = await _solarWatchService.UpdateCity(id, city);

        if (updatedCity == null)
        {
            return NotFound(new { Message = "City not found" });
        }

        return Ok(new { Message = "Update successful!", UpdatedCity = updatedCity });
    }

    [HttpDelete("cities/{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        var deletedCity = await _solarWatchService.DeleteCity(id);
        if (deletedCity != null)
        {
            return Ok(new { message = "Delete successful!", deletedCity });
        }

        return NotFound();
    }

    [HttpGet("cities/{id}"), Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> GetCityById(int id)
    {
        var city = await _solarWatchService.GetCityById(id);
        if (city != null)
        {
            return Ok(city);
        }

        return NotFound();
    }
    
    [HttpPost("sunrise-sunsets"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateSunriseSunset([FromBody] SunsetSunriseModel sunriseSunset)
    {
        if (sunriseSunset == null)
        {
            return BadRequest("Invalid data.");
        }

        await _solarWatchService.CreateSunriseSunset(sunriseSunset);
        return CreatedAtAction(nameof(GetSunriseSunsetById), new { id = sunriseSunset.Id }, sunriseSunset);
    }

    [HttpPut("sunrise-sunsets/{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateSunriseSunset(int id, [FromBody] UpdateSunsetSunriseDto sunriseSunset)
    {
        if (id != sunriseSunset.Id)
        {
            return BadRequest("ID mismatch.");
        }

        var updatedSS = await _solarWatchService.UpdateSunriseSunset(id,sunriseSunset);
        if (updatedSS != null)
        {
            return Ok(new {message = "Update successful!", updatedSS});
        }

        return NotFound();
    }

    [HttpDelete("sunrise-sunsets/{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSunriseSunset(int id)
    {

        var sunsetSunrise = await _solarWatchService.DeleteSunriseSunset(id);
        if (sunsetSunrise != null)
        {
            return Ok(new { message = "Delete successful!", sunsetSunrise });
        }
        return NotFound();
    }

    [HttpGet("sunrise-sunsets/{id}"), Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> GetSunriseSunsetById(int id)
    {
        var sunriseSunset = await _solarWatchService.GetSunriseSunsetById(id);
        if (sunriseSunset != null)
        {
            return Ok(sunriseSunset);
        }

        return NotFound();
    }
}