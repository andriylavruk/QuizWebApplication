using QuizApp.Shared.Models;

namespace QuizApp.Server.Data;

public static class DataSeeder
{
    public static void Seed(IApplicationBuilder applicationBuilder, IConfiguration configuration)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<DataContext>();

            context!.Database.EnsureCreated();

            if (!context.Roles.Any())
            {
                context.AddRange(new List<Role>()
            {
                new Role
                {
                    Name = "Administrator",
                },

                new Role
                {
                    Name = "Student"
                }
            });

                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var name = configuration.GetValue<string>("SuperUser:Name")!.ToString();
                var email = configuration.GetValue<string>("SuperUser:Email")!.ToString();
                var password = configuration.GetValue<string>("SuperUser:Password")!.ToString();

                var adminRole = context.Roles.FirstOrDefault(x => x.Name == "Administrator");

                context.Add(
                    new User
                    {
                        Email = email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                        Role = adminRole,
                        Group = null
                    });

                context.SaveChanges();
            }
        }
    }
}
