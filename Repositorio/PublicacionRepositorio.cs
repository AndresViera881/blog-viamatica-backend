using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Viamatica.Blog.Api.Data;
using Viamatica.Blog.Api.Modelos;
using Viamatica.Blog.Api.Repositorio.IRepositorio;

namespace Viamatica.Blog.Api.Repositorio
{
    public class PublicacionRepositorio : IPublicacionRepositorio
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PublicacionRepositorio(ApiDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

        }

        public bool DeletePublicacion(Publicacion publicacion)
        {
            _context.Publicaciones.Remove(publicacion);
            return Guardar();
        }

        public bool ExistPublicacionById(int publicacionId)
        {
            var existe = _context.Publicaciones.Any(p => p.PublicacionId == publicacionId);
            return existe;
        }

        public bool ExistPublicacionByTitle(string titulo)
        {
            var existe = _context.Publicaciones.Any(p => p.Titulo!.ToLower().Trim() == titulo.ToLower().Trim());
            return existe;
        }

        public Publicacion GetPublicacion(int publicacionId)
        {
            var publicacion = _context.Publicaciones.FirstOrDefault(p => p.PublicacionId == publicacionId);          
            return publicacion!;
        }

        public ICollection<Publicacion> GetPublicaciones()
        {
            var publicaciones = _context.Publicaciones.OrderBy(x => x.PublicacionId).ToList();
            return publicaciones;
        }

        public bool Guardar()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool PostPublicacion(Publicacion publicacion)
        {
            publicacion.FechaPublicacion = DateTime.Now;
            publicacion.Usuario = _httpContextAccessor!.HttpContext!.User!.Identity!.Name;
            _context.Publicaciones.Add(publicacion);
            return Guardar();
        }

        public bool PutPublicacion(Publicacion publicacion)
        {
            // Obtener la fecha de publicación original
            var fechaPublicacionOriginal = _context.Publicaciones
                .AsNoTracking()
                .Where(p => p.PublicacionId == publicacion.PublicacionId)
                .Select(p => p.FechaPublicacion)
                .FirstOrDefault();

            // Actualizar la fecha de actualización
            publicacion.FechaActualizacionPublicacion = DateTime.Now;

            // Restaurar la fecha de publicación original
            publicacion.FechaPublicacion = fechaPublicacionOriginal;

            // Actualizar la entidad en la base de datos
            _context.Publicaciones.Update(publicacion);

            // Guardar los cambios
            return Guardar();
        }
    }
}
