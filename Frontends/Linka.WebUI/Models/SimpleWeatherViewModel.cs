namespace Linka.WebUI.Models
{
    public class SimpleWeatherViewModel
    {
        public string City { get; set; } = "Istanbul";
        public float Temperature { get; set; }
        public string Description { get; set; } = "Sunny";
        public string BackgroundImage { get; set; } = "/images/weather/sunny.png";
    }
}
