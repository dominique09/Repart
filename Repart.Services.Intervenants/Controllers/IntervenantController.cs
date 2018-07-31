using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repart.Common.Commands.Intervenant;
using Repart.Services.Intervenants.Services;

namespace Repart.Services.Intervenants.Controllers
{
    [Route("")]
    public class IntervenantController : Controller
    {
        private readonly IIntervenantService _intervenantService;

        public IntervenantController(IIntervenantService intervenantService)
        {
            _intervenantService = intervenantService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "SUPER_ADMIN,GESTION_INTERVENANT")]
        public async Task<IActionResult> Create([FromBody] CreateIntervenant command)
            => Json(await _intervenantService.AddAsync(command.Firstname, command.Lastname, command.FormationId));

        [HttpPost("change-formation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "SUPER_ADMIN,GESTION_INTERVENANT")]
        public async Task<IActionResult> ChangeFormation([FromBody] ChangeFormation command)
            => Json(await _intervenantService.ChangeFormationAsync(command.IntervenantId, command.FormationId));

        [HttpGet("intervenants")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll()
            => Json(await _intervenantService.GetAllAsync());

        [HttpGet("intervenant/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Get(Guid id)
            => Json(await _intervenantService.GetAsync(id));
        
    }
}
