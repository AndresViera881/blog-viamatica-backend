using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Viamatica.Blog.Api.Data;
using Viamatica.Blog.Api.Modelos;
using Viamatica.Blog.Api.Modelos.Dtos;
using Viamatica.Blog.Api.Modelos.Dtos.Request;
using Viamatica.Blog.Api.Repositorio.IRepositorio;
using XSystem.Security.Cryptography;

namespace Viamatica.Blog.Api.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApiDbContext _context;
        private readonly Response _response;
        private readonly IMapper _mapper;
        private string claveSecreta;

        public UsuarioRepositorio(ApiDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _response = new();
            claveSecreta = configuration.GetValue<string>("ApiSettings:secreta");

        }

        public Usuario GetUsuario(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(x => x.UsuarioId == id)
                ?? throw new ArgumentNullException("Usuario no encontrado");
            return usuario;
        }

        public ICollection<Usuario> GetUsuarios()
        {
            var usuarios = _context.Usuarios.OrderBy(x => x.UsuarioId).ToList()
                ?? throw new ArgumentNullException("No existen usuarios");
            return usuarios;
        }

        public bool IsUniqueUser(string correo)
        {
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Correo == correo);
            if (usuario == null)
            {
                return true;
            }
            return false;
        }

        public async Task<UsuarioLoginRequestDto> Login(UsuarioLoginDto usuario)
        {
            var passwordEncriptado = ObtenerMD5(usuario.Password);
            var usuarioDb = await _context.Usuarios.FirstOrDefaultAsync(x => x.Correo!.ToLower() == usuario.Correo!.ToLower() && x.Password == passwordEncriptado);
            if (usuarioDb == null)
            {
                return new UsuarioLoginRequestDto()
                {
                    Token = "",
                    Usuario = null,
                };
            }

            //si existe el usuario
            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuarioDb.Correo),
                    //new Claim(ClaimTypes.Name, usuarioDb.Correo),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
            };

            var token = manejadorToken.CreateToken(tokenDescriptor);
            UsuarioLoginRequestDto usuarioLoginRequestDto = new UsuarioLoginRequestDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = usuarioDb,
            };

            return usuarioLoginRequestDto;

        }

        private string? ObtenerMD5(string valor)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] b = Encoding.UTF8.GetBytes(valor);
            b = md5.ComputeHash(b);
            string resp = "";
            for(int i = 0; i < b.Length; i++)
            {
                resp += b[i].ToString("x2").ToLower();
            }
            return resp;
        }

        public async Task<Usuario> RegistroUsuario(UsuarioRegistroDto usuarioRegistroDto)
        {
            var passwordEncriptado = ObtenerMD5(usuarioRegistroDto.Password);
            Usuario usuario = new Usuario()
            {
                Nombre = usuarioRegistroDto.Nombre,
                Correo = usuarioRegistroDto.Correo,
                Password = passwordEncriptado,
            }; 
            _context.Usuarios.Add(usuario);
            usuario.Password = passwordEncriptado;
            await _context.SaveChangesAsync();
            return usuario;

        }
    }
}
