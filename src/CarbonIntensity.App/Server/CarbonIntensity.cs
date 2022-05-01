using System;

namespace CarbonIntensity.App.Server
{
    public class CarbonIntensity
    {
        public int Key { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int RegionId { get; set; }
        public string DnoRegion { get; set; }
        public string Shortname { get; set; }
        public int Forecast { get; set; }
        public string ForecastDescription { get; set; }
        public double Gas { get; set; }
        public double Coal { get; set; }
        public double Biomass { get; set; }
        public double Nuclear { get; set; }
        public double Hydro { get; set; }
        public double Storage { get; set; }
        public double Imports { get; set; }
        public double Other { get; set; }
        public double Wind { get; set; }
        public double Solar { get; set; }
        public bool IsForecast { get; set; }

        public string PrintHeader()
        {
            return "From,To,RegionId,Shortname,Forecast,ForecastDescription,Gas,Coal," +
                   "Biomass,Nuclear,Hydro,Storage,Imports,Other,Wind,Solar,IsForecast";
        }

        public override string ToString()
        {
            return $"{From},{To},{RegionId},{Shortname},{Forecast}," +
                   $"{ForecastDescription},{Gas},{Coal},{Biomass}," +
                   $"{Nuclear},{Hydro},{Storage},{Imports},{Other}," +
                   $"{Wind},{Solar},{IsForecast}";
        }
    }
}