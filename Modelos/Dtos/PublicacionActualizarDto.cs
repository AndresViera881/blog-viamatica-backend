using System.ComponentModel.DataAnnotations;

namespace Viamatica.Blog.Api.Modelos.Dtos
{
    public class PublicacionActualizarDto
    {
        public int PublicacionId { get; set; }
        [Required]
        public string? Titulo { get; set; }
        [Required]
        public string? Contenido { get; set; }
        [Required]
        public string? Autor { get; set; }
        public DateTime FechaActualizacionPublicacion { get; set; }
        public string? RutaImagen { get; set; }
        public List<Comentario>? Comentarios { get; set; }
        // Usuario que publica, usuario logueado
        public string? Usuario { get; set; }
    }
}
