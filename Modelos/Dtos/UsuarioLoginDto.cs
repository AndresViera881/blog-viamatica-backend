using System.ComponentModel.DataAnnotations;

namespace Viamatica.Blog.Api.Modelos.Dtos
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Correo { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Password { get; set; }
    }
}
