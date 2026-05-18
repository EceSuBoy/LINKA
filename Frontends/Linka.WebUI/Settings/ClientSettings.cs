namespace Linka.WebUI.Settings
{
    public class ClientSettings
    {
        public Client LinkaVisitorClient { get; set; } 
        public Client LinkaManagerClient { get; set; }
        public Client LinkaAdminClient { get; set; }
    }
    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
