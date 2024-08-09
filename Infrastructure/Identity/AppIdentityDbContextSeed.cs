using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;


namespace Infrastructure.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var appUser = new AppUser()
                {
                    DisplayName = "shahd",
                    Email="shahd@gmail.com",
                    UserName= "shahd",
                    Address=new Address() 
                    {
                       FirstName= "shahd",
                       LastName="reda",
                       ZipCode="12456",
                       City="New York",
                       State="NY",
                       Street="10 st"
                    }
                };
                try
                {
                    var result = await userManager.CreateAsync(appUser, "Password_123");
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error: {error.Description}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
                //await userManager.CreateAsync(appUser,"password123");
            }
        }
    }
}
