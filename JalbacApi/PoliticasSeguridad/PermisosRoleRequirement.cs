using JalbacApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace JalbacApi.PoliticasSeguridad
{
    public class PermisosRoleRequirement : IAuthorizationRequirement
    {
        public string Controlador { get; }
        public List<Permiso> Permisos { get; }

        public PermisosRoleRequirement(string controlador, List<Permiso> permisos)
        {
            Controlador = controlador;
            Permisos = permisos;
        }
    }
}
