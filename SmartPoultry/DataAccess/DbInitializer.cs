using System;
using Microsoft.EntityFrameworkCore;

namespace SmartPoultry.DataAccess
{
    public class DbInitializer
    {
        public static void Initialize()
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                    else
                    {
                        context.Database.EnsureCreated(); 
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database Initialization Error: {ex.Message}");
                }
            }
        }
    }
}
