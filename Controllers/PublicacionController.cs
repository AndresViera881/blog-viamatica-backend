using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Viamatica.Blog.Api.Modelos;
using Viamatica.Blog.Api.Modelos.Dtos;
using Viamatica.Blog.Api.Repositorio.IRepositorio;

namespace Viamatica.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicacionController : ControllerBase
    {
        private readonly IPublicacionRepositorio _publicacionRepositorio;
        private readonly IMapper _mapper;

        public PublicacionController(IPublicacionRepositorio publicacionRepositorio, IMapper mapper)
        {
            _publicacionRepositorio = publicacionRepositorio;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetPublicaciones()
        {
            var publicaciones = _publicacionRepositorio.GetPublicaciones();
            var publicacionesDto = new List<PublicacionDto>();
            foreach (var publicacion in publicaciones)
            {
                publicacionesDto.Add(_mapper.Map<PublicacionDto>(publicacion));
            }
            return Ok(publicacionesDto);
        }

        [HttpGet("{publicacionId:int}", Name = "GetPublicacion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult GetPublicacion(int publicacionId)
        {
            var publicacion = _publicacionRepositorio.GetPublicacion(publicacionId);
            if (publicacion == null)
            {
                return NotFound();
            }
            var publicacionDto = _mapper.Map<PublicacionDto>(publicacion);
            return Ok(publicacionDto);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostPublicacion([FromBody] PublicacionCrearDto publicacionCrearDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if(publicacionCrearDto == null)
            {
                return BadRequest(ModelState);
            }

            if(_publicacionRepositorio.ExistPublicacionByTitle(publicacionCrearDto.Titulo))
            {
                ModelState.AddModelError("", $"La publicacion {publicacionCrearDto.Titulo} ya existe");
                return StatusCode(404, ModelState);
            }

            var publicacion = _mapper.Map<Publicacion>(publicacionCrearDto);
            if (!_publicacionRepositorio.PostPublicacion(publicacion))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro {publicacion.Titulo}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetPublicacion", new { publicacionId = publicacion.PublicacionId }, publicacion);
        }

        [Authorize]
        [HttpPatch("{publicacionId:int}", Name = "PutPublicacion")]       
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PutPublicacion(int publicacionId, [FromBody] PublicacionActualizarDto publicacionActualizarDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (publicacionActualizarDto == null || publicacionId != publicacionActualizarDto.PublicacionId)
            {
                return BadRequest(ModelState);
            }

            var publicacion = _mapper.Map<Publicacion>(publicacionActualizarDto);

            if (!_publicacionRepositorio.PutPublicacion(publicacion))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro {publicacion.Titulo}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        [Authorize]
        [HttpDelete("{publicacionId:int}", Name = "DeletePublicacion")]       
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeletePublicacion(int publicacionId)
        {
            if (!_publicacionRepositorio.ExistPublicacionById(publicacionId)) 
            {
                return NotFound();
            }

            var publicacion = _publicacionRepositorio.GetPublicacion(publicacionId);

            if (!_publicacionRepositorio.DeletePublicacion(publicacion))
            {
                ModelState.AddModelError("", $"Algo salio mal eliminando el registro {publicacion.Titulo}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
