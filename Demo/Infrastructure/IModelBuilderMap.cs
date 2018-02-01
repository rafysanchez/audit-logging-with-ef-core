using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure
{
    internal interface IModelBuilderMap
    {
        void Map(ModelBuilder builder);
    }
}