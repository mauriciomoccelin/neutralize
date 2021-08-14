using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Neutralize.EFCore
{
    public static class DbContextExtensions
    {
        public static ModelBuilder SetDefaultColumnTypeVarchar(this ModelBuilder modelBuilder)
        {
            const string defaultVarcharColumn = "varchar(255)";
                
            foreach (
                var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                    e => e.GetProperties().Where(p => p.ClrType == typeof(string)
                    )
                )
            ) property.SetColumnType(defaultVarcharColumn);

            return modelBuilder;
        }
    }
}