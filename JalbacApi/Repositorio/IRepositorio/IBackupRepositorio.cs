using JalbacApi.Models;

namespace JalbacApi.Repositorio.IRepositorio
{
    public interface IBackupRepositorio : IRepositorio<Backup>
    {
        Task<Backup> CrearBackup(Backup backup);
    }
}