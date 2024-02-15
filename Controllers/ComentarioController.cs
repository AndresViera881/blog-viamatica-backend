using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Viamatica.Blog.Api.Modelos;
using Viamatica.Blog.Api.Modelos.Dtos;
using Viamatica.Blog.Api.Repositorio;
using Viamatica.Blog.Api.Repositorio.IRepositorio;

namespace Viamatica.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentarioRepositorio _comentarioRepositorio;
        private readonly IMapper _mapper;

        public ComentarioController(IComentarioRepositorio comentarioRepositorio, IMapper mapper)
        {
            _comentarioRepositorio = comentarioRepositorio;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetComentarios()
        {
            var comentarios = _comentarioRepositorio.GetComentarios();
            var comentariosDto = new List<ComentarioDto>();
            foreach (var comentario in comentarios)
            {
                comentariosDto.Add(_mapper.Map<ComentarioDto>(comentario));
            }
            return Ok(comentariosDto);
        }


        [HttpGet("{publicacionId:int}", Name = "GetComentario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult GetComentario(int publicacionId)
        {
            var comentario = _comentarioRepositorio.GetComentario(publicacionId);
            if (comentario == null)
            {
                return NotFound();
            }
            var comentarioDto = _mapper.Map<ComentarioDto>(comentario);
            return Ok(comentarioDto);
        }


        [HttpGet("GetComentariosByPublicacion/{publicacionId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetComentariosByPublicacion(int publicacionId)
        {
            var comentarios = _comentarioRepositorio.GetComentariosByPublicacion(publicacionId);
            var comentariosDto = new List<ComentarioDto>();
            foreach (var comentario in comentarios)
            {
                comentariosDto.Add(_mapper.Map<ComentarioDto>(comentario));
            }
            return Ok(comentariosDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostComentario([FromBody] ComentarioDto comentarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (comentarioDto == null)
            {
                return BadRequest(ModelState);
            }

            var comentario = _mapper.Map<Comentario>(comentarioDto);
            if (!_comentarioRepositorio.PostComentario(comentario))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro {comentario.ComentarioId}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetComentario", new { publicacionId = comentario.PublicacionId }, comentario);
        }
    }
}
