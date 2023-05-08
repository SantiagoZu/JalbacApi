namespace JalbacApi.Models.Dto.PedidoDto
{
    public class PedidoCreateDto
    {
        public int IdCliente { get; set; }

        public int IdEstado { get; set; }

        public string FechaEntrega { get; set; }

    }
}
