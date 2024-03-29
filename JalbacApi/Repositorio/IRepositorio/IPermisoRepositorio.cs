﻿using JalbacApi.Models;
using JalbacApi.Models.Dto.PermisoDtos;

namespace JalbacApi.Repositorio.IRepositorio
{
    public interface IPermisoRepositorio : IRepositorio<Permiso>
    {
        Task<List<PermisoDto>> ListaPermisos(int idUsuario);
    }
}
