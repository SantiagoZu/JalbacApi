using JalbacApi.Models;
using JalbacApi.Models.Dto.UsuarioDtos;
using JalbacApi.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace JalbacApi.Repositorio
{
    public class UsuarioRepositorio : Repositorio<Usuario>, IUsuarioRepositorio
    {
        private readonly HttpContext _htppContext;
        private readonly BdJalbacContext _db;
        private string secretKey;
        private readonly IConfiguration _config;
        public UsuarioRepositorio(IHttpContextAccessor httpContext, BdJalbacContext db, IConfiguration configuration) : base(db)
        {
            _htppContext = httpContext.HttpContext;
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _config = configuration;
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

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto, HttpContext httpContext)
        {
            var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Correo.ToLower() == loginRequestDto.Correo.ToLower());


            if (usuario == null)
            {
                return new LoginResponseDto()
                {
                    isExitoso = false
                };
            }

            var passwordHasher = new PasswordHasher<Usuario>();

            var contrasenaCorrecta = passwordHasher.VerifyHashedPassword(null, usuario.Contrasena, loginRequestDto.Contrasena);

            CookieOptions cookie = new CookieOptions
            {
                Expires = DateTime.Now.AddMonths(1), // Establece la fecha de expiración de la cookie
                HttpOnly = true, // La cookie solo será accesible a través de HTTP (no JavaScript)
                Secure = false, // La cookie solo será enviada a través de conexiones seguras (HTTPS)
            };

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
                var FinalToken = tokenHandler.WriteToken(token);

                httpContext.Response.Cookies.Append("CookieJalbac", FinalToken);

                return new LoginResponseDto()
                {
                    isExitoso = true,
                    Token = FinalToken
                };
            }
            else
            {
                return new LoginResponseDto()
                {
                    isExitoso = false,
                    Token = ""
                };
            }
        }

        public async Task EnviarCorreo(CorreoDto correoDto)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("jalbacprueba@gmail.com", "JalbacSoft");
            var to = new EmailAddress(correoDto.Para);
            var subject = correoDto.Asunto;
            var plainTextContent = "";
            var htmlContent = correoDto.Contenido;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}