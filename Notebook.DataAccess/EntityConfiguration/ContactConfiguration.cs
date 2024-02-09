using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notebook.Domain.Entities;

namespace Notebook.DataAccess.EntityConfiguration
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(14);
            builder.HasIndex(c => c.PhoneNumber).IsUnique();

            builder.Property(c => c.Email)
                .HasMaxLength(256)
                .HasAnnotation("EmailTemplate", @"^[a-zA-Z0-9.]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$");
            builder.HasIndex(c => c.Email).IsUnique();
        }
    }
}
