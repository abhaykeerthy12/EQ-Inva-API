using EQ_Inva_API.Models;
using EQ_Inva_API.Models.ProjectModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EQ_Inva_API.Controllers
{
    [Authorize]
    [RoutePrefix("api/roles")]
    public class RolesController : ApiController
    {
        [HttpGet]
        // GET: /api/roles
        public IHttpActionResult GetRoles()
        {
            var roleContext = new ApplicationDbContext();

            var roles = roleContext.Roles
                        .Select(r => new
                        {
                            Id = r.Id,
                            Name = r.Name
                        });
            return Ok(roles);
        }

        [Route("GetUserRoles")]
        // GET: /api/roles
        public IHttpActionResult GetUserRoles()
        {
            var identityDbContext = new IdentityDbContext();

            var identityRole = identityDbContext.Users.Select(r => new {
                Id = r.Id,
                Roles = r.Roles,
                Email = r.Email
            });
            return Ok(identityRole);
        }

        // change a user permission
        [HttpPost]
        // POST: /api/roles
        public IHttpActionResult PostRoles(UserRoleClass userRole)
        {
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(userStore);
            if (userRole != null)
            {
                if(userRole.PreviousRole == "User")
                {
                    manager.AddToRole(userRole.Id, userRole.Role);
                }
                else
                {
                    manager.RemoveFromRole(userRole.Id, userRole.PreviousRole);
                    manager.AddToRole(userRole.Id, userRole.Role);
                }
            }
            return Ok();
        }
    }
}
