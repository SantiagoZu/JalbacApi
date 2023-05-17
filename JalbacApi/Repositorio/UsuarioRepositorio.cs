using JalbacApi.Models;
using JalbacApi.Models.Dto.UsuarioDtos;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace JalbacApi.Repositorio
{
    public class UsuarioRepositorio : Repositorio<Usuario>, IUsuarioRepositorio
    {
        private readonly BdJalbacContext _db;
        private string secretKey;
        public UsuarioRepositorio(BdJalbacContext db, IConfiguration configuration) : base(db)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public async Task<Usuario> CrearUsuario(Usuario usuario)
        {
            var passwordHasher = new PasswordHasher<Usuario>();

            var contrasenaEncriptada = passwordHasher.HashPassword(null, usuario.Contrasena);

            usuario.Contrasena = contrasenaEncriptada;

            await _db.Usuarios.AddAsync(usuario);   
            await _db.SaveChangesAsync();

            return usuario;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Correo.ToLower() == loginRequestDto.Correo.ToLower());


            if (usuario == null)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            var passwordHasher = new PasswordHasher<Usuario>();

            var contrasenaCorrecta = passwordHasher.VerifyHashedPassword(null, usuario.Contrasena, loginRequestDto.Contrasena);
            if (contrasenaCorrecta == PasswordVerificationResult.Success)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Role, usuario.IdRol.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return new LoginResponseDto()
                {
                    Token = tokenHandler.WriteToken(token),
                    Usuario = usuario,
                };
            }
            else
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    Usuario = null
                };
            }
        }
    }
}
