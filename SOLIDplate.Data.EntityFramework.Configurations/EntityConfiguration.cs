using System;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace SOLIDplate.Data.EntityFramework.Configurations
{
    public abstract class EntityConfiguration<TEntity, TEntityConfiguration> : EntityTypeConfiguration<TEntity>
        where TEntity : class, new()
        where TEntityConfiguration : EntityConfiguration<TEntity, TEntityConfiguration>, new()
    {
        protected string EntityTypeName => typeof(TEntity).Name;

        protected Action<ForeignKeyAssociationMappingConfiguration> MapTypeToForeignKeyColumnName(Type type)
        {
            return configuration => configuration.MapKey($"{type.Name}Id");
        }
    }
}
