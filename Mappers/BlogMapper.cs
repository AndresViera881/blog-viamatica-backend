using AutoMapper;
using Viamatica.Blog.Api.Modelos;
using Viamatica.Blog.Api.Modelos.Dtos;

namespace Viamatica.Blog.Api.Mappers
{
    public class BlogMapper: Profile
    {
        public BlogMapper()
        {
            CreateMap<Publicacion, PublicacionDto>().ReverseMap();
            CreateMap<Publicacion, PublicacionCrearDto>().ReverseMap();
            CreateMap<Publicacion, PublicacionActualizarDto>().ReverseMap();

            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuario, UsuarioRegistroDto>().ReverseMap();

            CreateMap<Comentario, ComentarioDto>().ReverseMap();
           
        }
    }
}
