using System;
using System.Collections.Generic;
using System.Linq;
using Heimdall.Client.Server;
using Microsoft.AspNetCore.Mvc;

namespace Heimdall.Client.Controllers
{
    [ApiController]
    [Route("api")]
    public class UsageController : ControllerBase
    {
        private readonly INationalGridRepository _repository;
        private readonly List<string> regions = new List<string>{"London"};

        public UsageController(INationalGridRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("forecast")]
        public Dictionary<string, List<CarbonIntensity>> GetForecast()
        {
            var forecasts = _repository.ForecastsByRegion
                .Where(f => regions.Contains(f.Key))
                .ToDictionary(x => x.Key, x => x.Value);

            return forecasts;
            // return _repository.ForecastsByRegion;
        }

        [HttpGet("current")]
        public List<CarbonIntensity> GetCurrent()
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
                .OrderBy(x => x.RegionId)
                .ToList();

            return forecasts;
        }
    }
}