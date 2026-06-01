namespace Linka.WebUI.Models
{
    public class UserLayoutNavbarViewModel
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FullName
        {
            get
            {
                var fullName = $"{FirstName} {LastName}".Trim();

                return string.IsNullOrWhiteSpace(fullName)
                    ? "LINKA User"
                    : fullName;
            }
        }

        public string Initials
        {
            get
            {
                var firstInitial = string.IsNullOrWhiteSpace(FirstName)
                    ? string.Empty
                    : FirstName.Trim()[0].ToString().ToUpper();

                var lastInitial = string.IsNullOrWhiteSpace(LastName)
                    ? string.Empty
                    : LastName.Trim()[0].ToString().ToUpper();

                var initials = $"{firstInitial}{lastInitial}";

                return string.IsNullOrWhiteSpace(initials)
                    ? "LU"
                    : initials;
            }
        }
    }
}
