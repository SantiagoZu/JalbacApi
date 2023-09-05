using JalbacApi.Models;
using JalbacApi.Repositorio.IRepositorio;

namespace JalbacApi.Repositorio
{
    public class BackupRepositorio : Repositorio<Backup>, IBackupRepositorio
    {
        private readonly BdJalbacContext _db;

        public BackupRepositorio(BdJalbacContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Backup> CrearBackup(Backup backup)
        {
            backup.FechaBackup = DateTime.Now;
            await _db.AddAsync(backup);
            await _db.SaveChangesAsync();

            return backup;
        }

    }
}