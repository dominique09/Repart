using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repart.Common.Commands.Formation;
using Repart.Services.Formations.Services;

namespace Repart.Services.Formations.Controllers
{
    [Route("")]
    public class FormationController : Controller
    {
        private readonly IFormationService _formationService;

        public FormationController(IFormationService formationService)
        {
            _formationService = formationService;
        }

        [HttpPost()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "SUPER_ADMIN,GESTION_FORMATION")]
        public async Task<IActionResult> Create([FromBody] CreateFormation command)
            => Json(await _formationService.AddAsync(command.Name, command.Abreviation, Guid.Parse(User.Identity.Name)));

        [HttpGet("formations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll()
            => Json(await _formationService.GetAllAsync());

        [HttpGet("formation/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get(Guid id)
            => Json(await _formationService.GetAsync(id));

        [HttpGet("formation/{id}/history")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetWithHistory(Guid id)
            => Json(await _formationService.GetWithHistoryAsync(id));

    }
}
