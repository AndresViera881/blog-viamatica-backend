using System.ComponentModel.DataAnnotations;

namespace Viamatica.Blog.Api.Modelos
{
    public class Comentario
    {
        [Key]
        public int ComentarioId { get; set; }
        public string? Contenido { get; set; }
        public int PublicacionId { get; set; }
        public Publicacion? Publicacion { get; set; }
    }
}
