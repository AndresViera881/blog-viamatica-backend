using Microsoft.EntityFrameworkCore;
using Viamatica.Blog.Api.Modelos;

namespace Viamatica.Blog.Api.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options): base(options)
        {
            
        }

        public DbSet<Publicacion> Publicaciones { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Publicacion)
                .WithMany(p => p.Comentarios)
                .HasForeignKey(c => c.PublicacionId);
        }


    }
}
