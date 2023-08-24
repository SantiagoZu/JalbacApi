namespace JalbacApi.Models.Dto.HisEstadoDetallePedidoDtos
{
    public class HistorialesDeDetalleDto
    {
        public int IdDetalle { get; set; }
        public IEnumerable<HisEstadoDetallePedido> HistorialDetalle { get; set; }
    }
}
