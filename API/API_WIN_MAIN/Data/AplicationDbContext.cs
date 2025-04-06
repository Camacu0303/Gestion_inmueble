using API_WIN_MAIN.Models;
using Microsoft.EntityFrameworkCore;

namespace API_WIN_MAIN
{
    //Permite la interciconexion entre el String y la aplicacion para conectarse a BD
    public class AplicationDbContext : DbContext
    {
        //Constructor vacio me permite inicializar cualquier clase.
        //En nuestro caso el constructor inicializa los datos de interconexion
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contrato>(entity =>
            {
                entity.HasOne(c => c.Propiedad)
                    .WithMany(p => p.Contratos)
                    .HasForeignKey(c => c.id_Propiedad);

                entity.HasOne(c => c.Cliente)
                    .WithMany()
                    .HasForeignKey(c => c.id_Cliente);

                entity.HasOne(c => c.EstadoContrato)
                    .WithMany()
                    .HasForeignKey(c => c.id_Estado);
            });
        }

        //Necesitamos que van hacer los set y gets.
        //La definicion de entidades y modelos
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoPropiedad> TiposPropiedad { get; set; }
        public DbSet<EstadoPropiedad> EstadosPropiedad { get; set; }
        public DbSet<Propiedad> Propiedades { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Agente> Agentes { get; set; }
        public DbSet<EstadoContrato> EstadosContrato { get; set; }
        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<EstadoPago> EstadosPago { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Visita> Visitas { get; set; }
        public DbSet<Calificacion> Calificaciones { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<EstadoNotificacion> EstadosNotificacion { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        //Y aca agregamos todas las que necesitemos
        //Usuarios, roles, categorias, facturas
    }
}
