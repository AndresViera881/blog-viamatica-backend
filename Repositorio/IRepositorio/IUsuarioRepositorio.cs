using Viamatica.Blog.Api.Modelos;
using Viamatica.Blog.Api.Modelos.Dtos;
using Viamatica.Blog.Api.Modelos.Dtos.Request;

namespace Viamatica.Blog.Api.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int id);
        bool IsUniqueUser(string correo);
        Task<UsuarioLoginRequestDto> Login(UsuarioLoginDto usuario);
        Task<Usuario> RegistroUsuario(UsuarioRegistroDto usuarioRegistroDto);

    }
}
