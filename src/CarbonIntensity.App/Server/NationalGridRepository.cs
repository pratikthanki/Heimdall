using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CarbonIntensity.App.Server
{
    public interface INationalGridRepository
    {
        Task TryExecuteAsync(CancellationToken cancellationToken);
        Dictionary<string, List<CarbonIntensity>> ForecastsByRegion { get; }
    }

    public class NationalGridRepository : INationalGridRepository
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly int HistoricalOffsetDays = 1;
        public Dictionary<string, List<CarbonIntensity>> ForecastsByRegion { get; private set; } = new();


        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            IgnoreNullValues = true,
            AllowTrailingCommas = true,
            IgnoreReadOnlyProperties = true
        };

        public NationalGridRepository(ILogger<NationalGridRepository> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(10) };
        }

        public async Task TryExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await RunAsync(cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogCritical(exception, exception.Message);
                }

                _logger.LogInformation("Delaying for 10 minutes before next run.");
                await Task.Delay(TimeSpan.FromMinutes(10), cancellationToken);
            }
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            var utcNow = DateTime.UtcNow;
            var utcNowRoundedToHourStart =
                new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, 0, 0, DateTimeKind.Utc);

            var forecastFromDate = utcNowRoundedToHourStart.AddMinutes(30);
            var historicalRange = (utcNowRoundedToHourStart.AddDays(-HistoricalOffsetDays), utcNowRoundedToHourStart);

            _logger.LogInformation("Fetching forecasts");

            var results = await GetCarbonIntensityAsync(forecastFromDate, historicalRange, cancellationToken);

            if (results.Count > 0)
            {
                _logger.LogInformation("Forecasts cache cleared");
                ForecastsByRegion = new Dictionary<string, List<CarbonIntensity>>();
            }

            var key = 1;
            foreach (var day in results)
            {
                foreach (var region in day.Regions)
                {
                    if (!ReferenceData.IsValidRegion(region.RegionId))
                    {
                        continue;
                    }

                    double TryGetFuelPercentage(string fuel)
                    {
                        var generationMix = region.GenerationMix?.FirstOrDefault(r => r.Fuel == fuel);
                        return generationMix?.Percentage ?? 0;
                    }

                    var ci = new CarbonIntensity();

                    try
                    {
                        ci = new CarbonIntensity()
                        {
                            Key = key++,
                            From = DateTime.ParseExact(day.From, "yyyy-MM-ddTHH:mmZ", new DateTimeFormatInfo()),
                            To = DateTime.ParseExact(day.To, "yyyy-MM-ddTHH:mmZ", new DateTimeFormatInfo()),
                            RegionId = region.RegionId,
                            DnoRegion = region.DnoRegion,
                            Shortname = region.Shortname,
                            Forecast = region.Intensity.Forecast,
                            ForecastDescription = region.Intensity.Index,
                            Gas = TryGetFuelPercentage(ReferenceData.Gas),
                            Coal = TryGetFuelPercentage(ReferenceData.Coal),
                            Biomass = TryGetFuelPercentage(ReferenceData.Biomass),
                            Nuclear = TryGetFuelPercentage(ReferenceData.Nuclear),
                            Hydro = TryGetFuelPercentage(ReferenceData.Hydro),
                            Storage = TryGetFuelPercentage(ReferenceData.Storage),
                            Imports = TryGetFuelPercentage(ReferenceData.Imports),
                            Other = TryGetFuelPercentage(ReferenceData.Other),
                            Wind = TryGetFuelPercentage(ReferenceData.Wind),
                            Solar = TryGetFuelPercentage(ReferenceData.Solar),
                            IsForecast =
                                DateTime.ParseExact(day.To, "yyyy-MM-ddTHH:mmZ", new DateTimeFormatInfo()) >=
                                forecastFromDate,
                        };
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "There was an error!");
                    }

                    if (ForecastsByRegion.TryGetValue(ci.Shortname, out var intensities))
                    {
                        intensities.Add(ci);
                    }
                    else
                    {
                        ForecastsByRegion.Add(ci.Shortname, new List<CarbonIntensity>() { ci });
                    }
                }
            }

            _logger.LogInformation("Forecasts cache populated");
        }

        private async Task<List<CarbonIntensityDay>> GetCarbonIntensityAsync(
            DateTime forecastFromDate,
            (DateTime from, DateTime to) historicalRange,
            CancellationToken cancellationToken)
        {
            var results = new List<CarbonIntensityDay>();

            var historicalUsage = await GetDataAsync<CarbonIntensityResponse>(
                ReferenceData.CreateHistoricalUrl(historicalRange.from, historicalRange.to),
                cancellationToken);

            if (historicalUsage?.Data is not null)
            {
                results.AddRange(historicalUsage.Data);
            }

            var forecastUsage = await GetDataAsync<CarbonIntensityResponse>(
                ReferenceData.CreateForecastUrl(forecastFromDate),
                cancellationToken);

            if (forecastUsage?.Data is not null)
            {
                results.AddRange(forecastUsage.Data);
            }

            _logger.LogInformation("Carbon intensities found: {Count}", results.Count);

            return results;
        }

        private async Task<T> GetDataAsync<T>(string url, CancellationToken cancellationToken)
        {
            var responseMessage =
                await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            _logger.LogInformation("Fetched _CarbonIntensities for {Url}", url);

            var responseStream = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
            var data = await JsonSerializer.DeserializeAsync<T>(
                responseStream, _jsonSerializerOptions, cancellationToken);

            _logger.LogInformation("Deserialized stream");

            return data;
        }
    }
}