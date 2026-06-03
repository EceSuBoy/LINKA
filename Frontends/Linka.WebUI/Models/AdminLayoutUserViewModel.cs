using System.Linq;

namespace Linka.WebUI.Models
{
    public class AdminLayoutUserViewModel
    {
        public UserDetailViewModel User { get; set; } =
            new UserDetailViewModel();

        public int MessageCount { get; set; }

        public int CommentCount { get; set; }

        public string FullName
        {
            get
            {
                var fullName =
                    $"{User.Name} {User.Surname}"
                        .Trim();

                if (!string.IsNullOrWhiteSpace(
                        fullName))
                {
                    return fullName;
                }

                if (!string.IsNullOrWhiteSpace(
                        User.Username))
                {
                    return User.Username;
                }

                return "LINKA User";
            }
        }

        public string Username =>
            string.IsNullOrWhiteSpace(
                User.Username)
                ? "user"
                : User.Username;

        public string Email =>
            string.IsNullOrWhiteSpace(
                User.Email)
                ? "No e-mail information"
                : User.Email;

        public string RoleName =>
            User.Roles?
                .FirstOrDefault()
            ?? "Authorized User";

        public string Initials
        {
            get
            {
                var nameInitial =
                    string.IsNullOrWhiteSpace(
                        User.Name)
                        ? string.Empty
                        : User.Name
                            .Substring(0, 1);

                var surnameInitial =
                    string.IsNullOrWhiteSpace(
                        User.Surname)
                        ? string.Empty
                        : User.Surname
                            .Substring(0, 1);

                var initials =
                    $"{nameInitial}{surnameInitial}"
                        .Trim()
                        .ToUpperInvariant();

                if (!string.IsNullOrWhiteSpace(
                        initials))
                {
                    return initials;
                }

                return string.IsNullOrWhiteSpace(
                        Username)
                    ? "LU"
                    : Username
                        .Substring(0, 1)
                        .ToUpperInvariant();
            }
        }
    }
}