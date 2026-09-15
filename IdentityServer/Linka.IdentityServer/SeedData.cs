using Linka.IdentityServer.Data;
using Linka.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Linq;

namespace Linka.IdentityServer
{
    public partial class SeedData
    {
        private static readonly string[] SystemRoles =
        {
            "Customer",
            "Manager",
            "Admin"
        };

        public static void EnsureSeedData(
            string connectionString)
        {
            var services =
                new ServiceCollection();

            services.AddLogging();

            services.AddDbContext<ApplicationDbContext>(
                options =>
                    options.UseSqlServer(
                        connectionString));

            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using var serviceProvider =
                services.BuildServiceProvider();

            using var scope =
                serviceProvider
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope();

            var context =
                scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();

            var userManager =
                scope.ServiceProvider
                    .GetRequiredService<
                        UserManager<ApplicationUser>>();

            var roleManager =
                scope.ServiceProvider
                    .GetRequiredService<
                        RoleManager<IdentityRole>>();

            foreach (var role in SystemRoles)
            {
                CreateRoleIfNotExists(
                    roleManager,
                    role);
            }
            var users =
                new[]
                {
                    new SeedUser
                    {
                        Username = "linkaadmin",
                        Email = "admin@linka.com",
                        Name = "LINKA",
                        Surname = "Admin",
                        Password = "Admin123!",
                        Role = "Admin"
                    },

                    new SeedUser
                    {
                        Username = "linkamanager",
                        Email = "manager@linka.com",
                        Name = "LINKA",
                        Surname = "Manager",
                        Password = "Manager123!",
                        Role = "Manager"
                    },

                    new SeedUser
                    {
                        Username = "kerem",
                        Email = "kerem@linka.com",
                        Name = "Kerem",
                        Surname = "Aydın",
                        Password = "Kerem123!",
                        Role = "Customer"
                    },

                    new SeedUser
                    {
                        Username = "mehmet",
                        Email = "mehmet@linka.com",
                        Name = "Mehmet",
                        Surname = "Demir",
                        Password = "Mehmet123!",
                        Role = "Customer"
                    },

                    new SeedUser
                    {
                        Username = "ayse",
                        Email = "ayse@linka.com",
                        Name = "Ayşe",
                        Surname = "Çelik",
                        Password = "Ayse123!",
                        Role = "Customer"
                    }
                };

            foreach (var user in users)
            {
                CreateOrUpdateUser(
                    userManager,
                    user);
            }

            foreach (var testUser in GetTestUsers())
            {
                CreateOrUpdateUser(
                    userManager,
                    testUser);
            }

            Log.Information(
                "LINKA identity seed completed successfully.");
        }

        private static void CreateRoleIfNotExists(
            RoleManager<IdentityRole> roleManager,
            string roleName)
        {
            var exists =
                roleManager
                    .RoleExistsAsync(roleName)
                    .GetAwaiter()
                    .GetResult();

            if (exists)
            {
                Log.Information(
                    "Role already exists: {Role}",
                    roleName);

                return;
            }

            var result =
                roleManager
                    .CreateAsync(
                        new IdentityRole(roleName))
                    .GetAwaiter()
                    .GetResult();

            if (!result.Succeeded)
            {
                throw new Exception(
                    string.Join(
                        " | ",
                        result.Errors.Select(
                            x => x.Description)));
            }

            Log.Information(
                "Role created: {Role}",
                roleName);
        }

        private static void CreateOrUpdateUser(
            UserManager<ApplicationUser> userManager,
            SeedUser seedUser)
        {
            var user =
                userManager
                    .FindByNameAsync(seedUser.Username)
                    .GetAwaiter()
                    .GetResult();

            if (user == null)
            {
                user =
                    new ApplicationUser
                    {
                        UserName =
                            seedUser.Username,

                        Email =
                            seedUser.Email,

                        Name =
                            seedUser.Name,

                        Surname =
                            seedUser.Surname,

                        EmailConfirmed =
                            true
                    };

                var createResult =
                    userManager
                        .CreateAsync(
                            user,
                            seedUser.Password)
                        .GetAwaiter()
                        .GetResult();

                if (!createResult.Succeeded)
                {
                    throw new Exception(
                        $"User could not be created: " +
                        $"{seedUser.Username}. " +
                        string.Join(
                            " | ",
                            createResult.Errors.Select(
                                x => x.Description)));
                }

                Log.Information(
                    "User created: {Username}",
                    seedUser.Username);
            }
            else
            {
                user.Email =
                    seedUser.Email;

                user.Name =
                    seedUser.Name;

                user.Surname =
                    seedUser.Surname;

                user.EmailConfirmed =
                    true;

                var updateResult =
                    userManager
                        .UpdateAsync(user)
                        .GetAwaiter()
                        .GetResult();

                if (!updateResult.Succeeded)
                {
                    throw new Exception(
                        $"User could not be updated: " +
                        $"{seedUser.Username}. " +
                        string.Join(
                            " | ",
                            updateResult.Errors.Select(
                                x => x.Description)));
                }

                Log.Information(
                    "User already exists: {Username}",
                    seedUser.Username);

                var resetToken =
    userManager
        .GeneratePasswordResetTokenAsync(user)
        .GetAwaiter()
        .GetResult();

                var passwordResult =
                    userManager
                        .ResetPasswordAsync(
                            user,
                            resetToken,
                            seedUser.Password)
                        .GetAwaiter()
                        .GetResult();

                if (!passwordResult.Succeeded)
                {
                    throw new Exception(
                        $"Password could not be reset: " +
                        $"{seedUser.Username}. " +
                        string.Join(
                            " | ",
                            passwordResult.Errors.Select(
                                x => x.Description)));
                }

                Log.Information(
                    "Password reset: {Username}",
                    seedUser.Username);
            }

            var currentRoles =
                userManager
                    .GetRolesAsync(user)
                    .GetAwaiter()
                    .GetResult();

            foreach (
                var currentRole in
                currentRoles.Where(
                    role =>
                        SystemRoles.Contains(role) &&
                        role != seedUser.Role))
            {
                userManager
                    .RemoveFromRoleAsync(
                        user,
                        currentRole)
                    .GetAwaiter()
                    .GetResult();
            }

            var alreadyInRole =
                userManager
                    .IsInRoleAsync(
                        user,
                        seedUser.Role)
                    .GetAwaiter()
                    .GetResult();

            if (!alreadyInRole)
            {
                var roleResult =
                    userManager
                        .AddToRoleAsync(
                            user,
                            seedUser.Role)
                        .GetAwaiter()
                        .GetResult();

                if (!roleResult.Succeeded)
                {
                    throw new Exception(
                        $"Role could not be assigned: " +
                        $"{seedUser.Username}. " +
                        string.Join(
                            " | ",
                            roleResult.Errors.Select(
                                x => x.Description)));
                }

                Log.Information(
                    "Role assigned: {Username} -> {Role}",
                    seedUser.Username,
                    seedUser.Role);
            }
        }

        private class SeedUser
        {
            public string Username { get; set; } =
                string.Empty;

            public string Email { get; set; } =
                string.Empty;

            public string Name { get; set; } =
                string.Empty;

            public string Surname { get; set; } =
                string.Empty;

            public string Password { get; set; } =
                string.Empty;

            public string Role { get; set; } =
                string.Empty;
        }
    }
}