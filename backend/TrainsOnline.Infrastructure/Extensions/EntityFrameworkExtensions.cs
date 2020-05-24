namespace TrainsOnline.Infrastructure.Extensions
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;

    public static class EntityFrameworkExtensions
    {
        public static string GetTableName(this IModel model, Type entityType)
        {
            IEntityType efEntityType = model.FindEntityType(entityType);
            string tableName = efEntityType.GetTableName();

            return tableName;
        }
    }
}
