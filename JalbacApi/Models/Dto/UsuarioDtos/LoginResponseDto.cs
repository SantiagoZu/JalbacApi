namespace JalbacApi.Models.Dto.UsuarioDtos
{
    public class LoginResponseDto
    {
        public LoginResponseDto()
        {
        }

        public Usuario Usuario { get; set; }
        public string Token { get; set; }
    }
}
