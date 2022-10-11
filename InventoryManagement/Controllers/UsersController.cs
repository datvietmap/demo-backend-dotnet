using InventoryManagement.Dtos;
using InventoryManagement.Entities;
using InventoryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Graph;
using static InventoryManagement.Helpers.Constants;

namespace InventoryManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UsersController(IConfiguration configuration, IUserService userService, IRoleService roleService)
        {
            _configuration = configuration;
            _userService = userService;
            _roleService = roleService;
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <param name="id">Id of User</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var user = await _userService.Get(Guid.Parse(id));
                if (user != null)
                {
                    return Ok(new JsonResult(new
                    {
                        user.FullName,
                        user.EmailAddress,
                        user.RoleId
                    }));
                }
                else
                {
                    throw new Exception("User not found.");
                }
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

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            try
            {
                var success = await _userService.Create(request);
                if (success)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
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

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="authenticate"></param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate(AuthenticateModel authenticate)
        {
            try
            {
                var user = await _userService.Authenticate(authenticate.Username, authenticate.Password);
                if (user != null)
                {
                    return Ok(new JsonResult(new
                    {
                        user.Id,
                        user.FullName,
                        user.EmailAddress,
                        user.RoleId
                    }));
                }
                else
                {
                    throw new Exception("Unauthenticate.");
                }
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

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="authenticate"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(UpdateUserRequest request)
        {
            try
            {
                var success = await _userService.UpdateUser(request);
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
