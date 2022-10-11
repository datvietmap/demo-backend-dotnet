using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Entities;
using InventoryManagement.Dtos;
using InventoryManagement.Services;
using static InventoryManagement.Helpers.Constants;
using Microsoft.Graph;

namespace InventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutRole(UpdateRoleRequest roleRequest)
        {
            try
            {
                var success = await _roleService.UpdateRole(roleRequest);
                return Ok(new JsonResult(new
                {
                    success
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResult(new
                {
                    ex.Message,
                    innerException = ex.InnerException?.Message,
                    ex.StackTrace
                }));
            }
        }

        // POST: api/Roles
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> PostRole(CreateRoleRequest roleRequest)
        {
            try
            {
                var success = await _roleService.Create(roleRequest);
                return Ok(new JsonResult(new
                {
                    success
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new JsonResult(new
                {
                    ex.Message,
                    innerException = ex.InnerException?.Message,
                    ex.StackTrace
                }));
            }
        }
    }
}
