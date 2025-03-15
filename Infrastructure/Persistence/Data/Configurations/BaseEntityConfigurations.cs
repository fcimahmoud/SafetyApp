
namespace Persistence.Data.Configurations
{
    internal class BaseEntityConfigurations<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(B => B.Id).ValueGeneratedOnAdd();   // If Key (Id) is an Numeric Type It Will Use The Identity Column (1,1), If it isn't Will Generate a New Guid
        }
    }
}
