using System;
using Microsoft.EntityFrameworkCore;

namespace SmartPoultry.DataAccess
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            try
            {
                context.Database.Migrate(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database migration failed: {ex.Message}");
            }
        }
    }
}
