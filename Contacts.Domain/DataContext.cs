using Contacts.Domain.Models.Contacts;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Domain
{
    public class DataContext: DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        // Overriding the OnModelCreating method to configure the model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the Contact entity to map to the "Contacts.Contact" table
            modelBuilder.Entity<Contact>(entity => 
            { 
                entity.ToTable("Contacts", "Contacts"); 
            });

            // Changing cascade delete behavior to restrict delete for all foreign keys
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);
        }
    }
}