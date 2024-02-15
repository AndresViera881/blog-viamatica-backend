namespace Viamatica.Blog.Api.Modelos.Dtos.Request
{
    public class UsuarioLoginRequestDto
    {
        public Usuario? Usuario { get; set; }
        public string? Token { get; set; }
    }
}
