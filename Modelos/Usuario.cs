using System.ComponentModel.DataAnnotations;

namespace Viamatica.Blog.Api.Modelos
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public string? Password { get; set; }
    }
}
