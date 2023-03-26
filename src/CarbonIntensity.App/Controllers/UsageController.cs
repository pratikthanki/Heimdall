using System.Collections.Generic;
using CarbonIntensity.App.Server;
using Microsoft.AspNetCore.Mvc;

namespace CarbonIntensity.App.Controllers;

[ApiController]
[Route("usage")]
public class UsageController : ControllerBase
{
    private readonly INationalGridRepository _repository;

    // Limit regions for testing
    private readonly List<string> _regions = new() { "London" };

    public UsageController(INationalGridRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("forecast")]
    public Dictionary<string, List<Server.CarbonIntensity>> GetForecast()
    {
        return _repository.GetForecastsByRegion();
    }

    [HttpGet("current")]
    public IEnumerable<Server.CarbonIntensity> GetCurrent()
    {
        return _repository.GetCurrentUsageForRegions();
    }
}