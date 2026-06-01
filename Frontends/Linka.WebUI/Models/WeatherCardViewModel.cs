namespace Linka.WebUI.Models
{
    public class WeatherCardViewModel
    {
        public string City { get; set; } = "Istanbul";
        public float Temperature { get; set; }
        public float FeelsLike { get; set; }
        public int Humidity { get; set; }
        public float WindSpeed { get; set; }

        public string Description { get; set; } = "Weather information";
        public string WeatherCondition { get; set; } = "Default";
        public string Icon { get; set; } = "";

        public string BackgroundImage { get; set; } =
            "/images/weather/default.jpg";

        public string TemperatureUnit { get; set; } = "°F";

        public bool IsAvailable { get; set; }
    }
}
