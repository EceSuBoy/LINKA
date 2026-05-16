namespace Linka.WebUI.Settings
{
    public class ClientSettings
    {
        public Client LinkaVisitorId { get; set; } 
        public Client LinkaManagerId { get; set; }
        public Client LinkaAdminId { get; set; }
    }
    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
