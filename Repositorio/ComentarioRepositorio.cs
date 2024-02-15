using AutoMapper;
using Viamatica.Blog.Api.Data;
using Viamatica.Blog.Api.Modelos;
using Viamatica.Blog.Api.Repositorio.IRepositorio;

namespace Viamatica.Blog.Api.Repositorio
{
    public class ComentarioRepositorio : IComentarioRepositorio
    {

        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ComentarioRepositorio(ApiDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

        }
        public Comentario GetComentario(int publicacionId)
        {
            var comentario = _context.Comentarios.FirstOrDefault(p => p.PublicacionId == publicacionId);
            return comentario!;
        }

        public ICollection<Comentario> GetComentarios()
        {
            var comentarios = _context.Comentarios.OrderBy(x => x.ComentarioId).ToList();
            return comentarios;
        }

        public ICollection<Comentario> GetComentariosByPublicacion(int publicacionId)
        {
            var comentarios = _context.Comentarios.Where(p => p.PublicacionId == publicacionId).ToList();
            return comentarios;
        }

        public bool Guardar()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool PostComentario(Comentario comentario)
        {
            _context.Comentarios.Add(comentario);
            return Guardar();
        }
    }
}
