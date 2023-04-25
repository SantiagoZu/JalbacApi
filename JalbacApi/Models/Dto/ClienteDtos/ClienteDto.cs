namespace JalbacApi.Models.Dto.ClienteDtos
{
    public class ClienteDto
    {
        public int IdCliente { get; set; }
        public string Documento { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Telefono { get; set; }

        public bool? Estado { get; set; }
    }
}
