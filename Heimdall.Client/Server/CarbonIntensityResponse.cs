using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Heimdall.Client.Server
{
    public class CarbonIntensityResponse
    {
        public List<CarbonIntensityDay> Data { get; set; }
    }

    public class CarbonIntensityDay
    {
        public string From { get; set; }
        public string To { get; set; }
        public List<Region> Regions { get; set; }
    }

    public class Region
    {
        public int RegionId { get; set; }
        public string DnoRegion { get; set; }
        public string Shortname { get; set; }
        public Intensity Intensity { get; set; }
        public List<Generationmix> GenerationMix { get; set; }
    }

    public class Intensity
    {
        public int Forecast { get; set; }
        public string Index { get; set; }
    }

    public class Generationmix
    {
        public string Fuel { get; set; }

        [JsonPropertyName("perc")] public double Percentage { get; set; }
    }
}