using System;
using Microsoft.EntityFrameworkCore;

namespace SmartPoultry.DataAccess
{
    public static class DbInitializer
    {
        public static void ApplyMigrations(AppDbContext context)
        {
            context.Database.Migrate();
        }
    }
}
