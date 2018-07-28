using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repart.Common.Commands.Identity;
using Repart.Services.Identity.Domain.Repositories;
using Repart.Services.Identity.Services;

namespace Repart.Services.Identity.Controllers
{
    [Route("")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        
        public AccountController(IUserService userService, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        [HttpPost("register")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SUPER_ADMIN,CREER_UTILISATEUR")]
        public async Task<IActionResult> Register([FromBody] RegisterUser command)
        => Json(await _userService.RegisterAsync(command.Email, command.Password, command.Name, command.Roles));
        
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] AuthenticateUser command)
            => Json(await _userService.LoginAsync(command.Email, command.Password));

        [HttpGet("users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SUPER_ADMIN,CREER_UTILISATEUR,GESTION_UTILISATEUR,CONSULTATION_UTILISATEUR")]
        public async Task<IActionResult> GetAllUsers()
            => Json(await _userService.GetAll());

        [HttpGet("user/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var roles = "SUPER_ADMIN,CREER_UTILISATEUR,GESTION_UTILISATEUR,CONSULTATION_UTILISATEUR";
            if (!roles.Split(',').Any(r => User.IsInRole(r)) && User.Identity.Name != id.ToString())
                return Unauthorized();



            return Json(await _userService.GetAsync(id));
        }
        
        [HttpGet("user/toggle-active/{id}")]
        public async Task<IActionResult> ToggleActive(Guid id)
            => Json(await _userService.ToggleActive(id));

        [HttpGet("roles")]
        public async Task<IActionResult> GetAll()
            => Json(await _roleRepository.GetAll());

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] ModifyUserRole command)
            => Json(await _userService.AddToRole(command.UserId, command.RoleId));

        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole([FromBody] ModifyUserRole command)
            => Json(await _userService.RemoveFromRole(command.UserId, command.RoleId));

        [HttpGet("Test")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult test()
        {
            return Json(User.Identity.Name);
        }

    }
}
