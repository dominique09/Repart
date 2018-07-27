using System;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Register([FromBody] RegisterUser command)
        => Json(await _userService.RegisterAsync(command.Email, command.Password, command.Name, command.Roles));
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateUser command)
            => Json(await _userService.LoginAsync(command.Email, command.Password));

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
            => Json(await _userRepository.GetAll());

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
            => Json(await _userService.GetAsync(id));

        [HttpGet("roles")]
        public async Task<IActionResult> GetAll()
            => Json(await _roleRepository.GetAll());

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] ModifyUserRole command)
            => Json(await _userService.AddToRole(command.UserId, command.RoleId));

        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole([FromBody] ModifyUserRole command)
            => Json(await _userService.RemoveFromRole(command.UserId, command.RoleId));
    }
}
