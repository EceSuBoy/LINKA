namespace Linka.WebUI.Models
{
    public class HeaderUserViewModel
    {
        public bool IsAuthenticated { get; set; }

        public string FullName { get; set; } = string.Empty;

        public bool CanAccessAdminPanel { get; set; }
    }
}
