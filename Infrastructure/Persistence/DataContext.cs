using Domain.Entities.PSE.Epayco;
using Domain.Entities.PSE.ZonaPagos;
using Domain.Entities.Recaudos;
using Microsoft.EntityFrameworkCore;

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
