using Newtonsoft.Json;

namespace xamarinCSlab09
{


    public struct WeatherOBJ
    {
        [JsonProperty("weather")] public Weather[] Weather { get; set; }
        public string Description => Weather[0].Description;
        [JsonProperty("main")] public Main Main { get; set; }
        public double Temp => Main.Temp;

        [JsonProperty("sys")] public Sys Sys { get; set; }
        public string Country => Sys.Country;

        [JsonProperty("name")] public string Name { get; set; }


    }

    public class Weather
    {
        [JsonProperty("description")] public string Description { get; set; }
    }

    public class Sys
    {
        [JsonProperty("country")] public string Country { get; set; }
    }

    public class Main
    {
        public double _temp { get; set; }

        [JsonProperty("temp")]
        public double Temp
        {
            get { return _temp; }
            set { _temp = value - 273; }
        }
    }
}