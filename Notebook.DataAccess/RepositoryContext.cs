using Microsoft.EntityFrameworkCore;
using Notebook.DataAccess.EntityConfiguration;
using Notebook.Domain.Entities;

namespace Notebook.DataAccess
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) 
            : base(options) 
        {
        }

        public DbSet<Contact> Contacts {  get; set; }
        public DbSet<Address> Address { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
        }
    }
}
