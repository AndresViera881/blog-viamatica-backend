using System.ComponentModel.DataAnnotations;

namespace Viamatica.Blog.Api.Modelos.Dtos
{
    public class PublicacionCrearDto
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Titulo { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Contenido { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string? Autor { get; set; }
        public DateTime FechaPublicacion { get; set; } = DateTime.Now;
        public string? RutaImagen { get; set; }
        // Usuario que publica, usuario logueado
        public string? Usuario { get; set; }
    }
}
