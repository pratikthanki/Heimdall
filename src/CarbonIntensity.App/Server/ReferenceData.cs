using System;
using System.Collections.Generic;
using System.Linq;

namespace CarbonIntensity.App.Server
{
    public static class ReferenceData
    {
        public const string DateFormat = "yyyy-MM-ddTHH:mm:ssZ";

        public const string Gas = "gas";
        public const string Coal = "coal";
        public const string Biomass = "biomass";
        public const string Nuclear = "nuclear";
        public const string Hydro = "hydro";
        public const string Storage = "storage";
        public const string Imports = "imports";
        public const string Other = "other";
        public const string Wind = "wind";
        public const string Solar = "solar";

        public static bool IsValidRegion(int region)
        {
            return _regions.Select(x => x.id).Contains(region);
        }

        // https://carbon-intensity.github.io/api-definitions/#region-list
        private static List<(int id, string name)> _regions = new List<(int, string)>()
        {
            (1, "North Scotland"),
            (2, "South Scotland"),
            (3, "North West England"),
            (4, "North East England"),
            (5, "Yorkshire"),
            (6, "North Wales"),
            (7, "South Wales"),
            (8, "West Midlands"),
            (9, "East Midlands"),
            (10, "East England"),
            (11, "South West England"),
            (12, "South England"),
            (13, "London"),
            (14, "South East England"),
            // (15, "England"),
            // (16, "Scotland"),
            // (17, "Wales"),
            // (18, "GB"),
        };
        
        public static string CreateForecastUrl(DateTime dateTime)
        {
            // https://carbon-intensity.github.io/api-definitions/#get-regional-intensity-from-fw48h
            return $"https://api.carbonintensity.org.uk/regional/intensity/{dateTime.ToString(DateFormat)}/fw48h/";
        }

        public static string CreateHistoricalUrl(DateTime from, DateTime to)
        {
            return
                $"https://api.carbonintensity.org.uk/regional/intensity/{from.ToString(DateFormat)}/{to.ToString(DateFormat)}";
        }
    }
}