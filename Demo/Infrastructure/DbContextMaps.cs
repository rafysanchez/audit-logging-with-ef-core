using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure
{
    internal static class DbContextMaps
    {
        public static void Map(this ModelBuilder modelBuilder, IModelBuilderMap config)
        {
            config.Map(modelBuilder);
        }
    }
}