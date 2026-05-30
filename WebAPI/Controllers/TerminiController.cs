using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domen;
using WebAPI.Services;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TerminiController : ControllerBase
    {
        private readonly ITerminService _terminService;

        public TerminiController(ITerminService terminService)
        {
            _terminService = terminService;
        }

        /// <summary>
        /// Dodaj novi termin treninga
        /// </summary>
        [HttpPost]
        public IActionResult CreateTermin([FromBody] TerminTreninga termin, [FromQuery] string statusOpis)
        {
            try
            {
                if (termin == null)
                    return BadRequest(new { message = "Podaci termina su obavezni." });

                var idAdminClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (idAdminClaim == null || !int.TryParse(idAdminClaim.Value, out int idAdmin))
                    return Unauthorized(new { message = "Nemoguće proveriti administratora." });

                _terminService.CreateTermin(termin, idAdmin, statusOpis ?? "Zakazan");
                return CreatedAtAction(nameof(CreateTermin), new { termin.IdTermin }, termin);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
