namespace JalbacApi.Models.Dto.UsuarioDtos
{
    public class LoginResponseDto
    {
        public LoginResponseDto()
        {
        }

        public bool isExitoso { get; set; }
        public string Token { get; set; }
    }
}
