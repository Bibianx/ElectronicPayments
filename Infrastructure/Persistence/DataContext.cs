using Domain.Entities.Epayco;
using Domain.Entities.ZonaPagos;

namespace Models
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<RECAUDOSBANCOS> RECAUDOSBANCOS { get; set; }
        public DbSet<HISTORIALZP> HISTORIALZP { get; set; }
        public DbSet<INTENTOSZP> INTENTOSZP { get; set; }
        public DbSet<EPAYCO> EPAYCO { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<INTENTOSZP>().Property(x => x.numero_ticket_cobol).UseIdentityColumn();
            base.OnModelCreating(modelBuilder);
        }
    }

}
