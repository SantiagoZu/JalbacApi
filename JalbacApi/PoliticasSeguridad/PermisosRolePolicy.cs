//using Microsoft.AspNetCore.Authorization;
//using System.Security.Claims;

//namespace JalbacApi.PoliticasSeguridad
//{
//    public class PermisosRolePolicy : AuthorizationHandler<PermisosRoleRequirement>
//    {
//        private readonly IHttpContextAccessor _httpContextAccessor;

//        public PermisosRolePolicy(IHttpContextAccessor httpContextAccessor)
//        {
//            _httpContextAccessor = httpContextAccessor;
//        }

//        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermisosRoleRequirement requirement)
//        {
//            var rolePermisos = requirement.Permisos;

//            var userRoles = _httpContextAccessor.HttpContext.User.Claims
//                .Where(c => c.Type == ClaimTypes.Role)
//                .Select(c => c.Value)
//                .ToList();

//            foreach (var role in userRoles)
//            {
//                var rolePermissions = rolePermisos.Where(rp => rp.IdRol == role.IdRol).ToList();

//                foreach (var permiso in rolePermissions)
//                {
//                    if (permiso.NombreControlador == requirement.Controlador)
//                    {
//                        context.Succeed(requirement);
//                        return Task.CompletedTask;
//                    }
//                }
//            }

//            return Task.CompletedTask;
//        }
//    }
//}
