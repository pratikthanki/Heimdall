using System;
using System.Collections.Generic;
using System.Linq;
using CarbonIntensity.App.Server;
using Microsoft.AspNetCore.Mvc;

namespace CarbonIntensity.App.Controllers
{
    [ApiController]
    [Route("api")]
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
            var forecasts = _repository.ForecastsByRegion
                .Where(f => _regions.Contains(f.Key))
                .ToDictionary(x => x.Key, x => x.Value);

            return forecasts;
            // TODO
            // return _repository.ForecastsByRegion;
        }

        [HttpGet("current")]
        public IEnumerable<Server.CarbonIntensity> GetCurrent()
        {
            var utcNow = DateTime.UtcNow;
            var intervalStart =
                new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, 0, 0, DateTimeKind.Utc);

            // Round to the most recent completed 30-min period
            if (utcNow.Minute <= 30)
            {
                intervalStart = intervalStart.AddMinutes(-30);
            }

            var intervalEnd = intervalStart.AddMinutes(30);

            var forecasts = _repository.ForecastsByRegion
                .SelectMany(x => x.Value.Where(r => r.From >= intervalStart && r.To <= intervalEnd))
                .OrderBy(x => x.RegionId);

            return forecasts;
        }
    }
}