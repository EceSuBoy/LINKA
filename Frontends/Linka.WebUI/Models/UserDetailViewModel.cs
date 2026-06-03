using System.Collections.Generic;

namespace Linka.WebUI.Models
{
    public class UserDetailViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
        public List<string> Roles { get; set; } =
            new List<string>();
    }
}