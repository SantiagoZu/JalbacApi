using System.Linq.Expressions;

namespace JalbacApi.Repositorio.IRepositorio
{
        public interface IRepositorio<T> where T : class
        {
            Task Crear(T obj);
            Task Editar(T obj);
            Task<List<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, bool tracked = true, string incluirPropiedades = null);
            Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true, string incluirPropiedades = null);
            Task<List<T>> Consultar(Expression<Func<T, bool>> filtor = null);
            Task Remover(T obj);
            Task Grabar();

        }
}
