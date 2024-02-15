using Viamatica.Blog.Api.Modelos;

namespace Viamatica.Blog.Api.Repositorio.IRepositorio
{
    public interface IComentarioRepositorio
    {
        ICollection<Comentario> GetComentarios();
        ICollection<Comentario> GetComentariosByPublicacion(int publicacionId);
        Comentario GetComentario(int publicacionId);
        bool PostComentario(Comentario comentario);
        bool Guardar();
    }
}
