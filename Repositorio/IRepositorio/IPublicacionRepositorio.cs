using Viamatica.Blog.Api.Modelos;

namespace Viamatica.Blog.Api.Repositorio.IRepositorio
{
    public interface IPublicacionRepositorio
    {
        ICollection<Publicacion> GetPublicaciones();
        Publicacion GetPublicacion(int publicacionId);
        bool ExistPublicacionByTitle(string titulo);
        bool ExistPublicacionById(int publicacionId);

        bool PostPublicacion(Publicacion publicacion);
        bool PutPublicacion(Publicacion publicacion);
        bool DeletePublicacion(Publicacion publicacion);
        bool Guardar();
    }
}
