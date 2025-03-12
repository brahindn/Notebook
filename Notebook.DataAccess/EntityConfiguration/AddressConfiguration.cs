using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notebook.Domain.Entities;

namespace Notebook.DataAccess.EntityConfiguration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(a => a.AddressType).IsRequired();

            builder.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(a => a.Region)
                .HasMaxLength(30);

            builder.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.BuildingNumber).IsRequired();
        }
    }
}
