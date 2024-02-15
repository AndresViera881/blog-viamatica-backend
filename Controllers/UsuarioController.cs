using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Viamatica.Blog.Api.Modelos;
using Viamatica.Blog.Api.Modelos.Dtos;
using Viamatica.Blog.Api.Repositorio.IRepositorio;

namespace Viamatica.Blog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IMapper _mapper;
        private readonly Response _response;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio, IMapper mapper)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
            _response = new();
        }

        [HttpPost("RegistroUsuario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistroUsuario(UsuarioRegistroDto usuarioRegistroDto)
        {
            var validarUsuarioUnico = _usuarioRepositorio.IsUniqueUser(usuarioRegistroDto.Correo);
            if (!validarUsuarioUnico)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Success = false;
                _response.ErrorMessages.Add("El correo ya existe");
                return BadRequest(_response);
            }

            var usuario = await _usuarioRepositorio.RegistroUsuario(usuarioRegistroDto);
            if (usuario == null)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.Success = false;
                _response.ErrorMessages.Add("Error al registrar el usuario");
                return StatusCode(500, _response);
            }

            _response.StatusCode = HttpStatusCode.Created;
            _response.Success = true;
            return Ok(_response);

        }


        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var loginData = await _usuarioRepositorio.Login(usuarioLoginDto);
            if (loginData.Usuario == null || string.IsNullOrEmpty(loginData.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Success = false;
                _response.ErrorMessages.Add("Usuario o contraseña incorrecta");
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.Success = true;
            _response.Result = loginData;
            return Ok(_response);
        }


    }
}
