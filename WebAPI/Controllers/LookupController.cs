using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domen;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class LookupController : ControllerBase
    {
        private readonly ILookupService _lookupService;

        public LookupController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        /// <summary>
        /// Preuzmi sve tipove članarina (za padajuće menije)
        /// </summary>
        [HttpGet("tipovi-clanarina")]
        public ActionResult<List<TipClanarine>> GetTipoviClanarina()
        {
            try
            {
                return Ok(_lookupService.GetAllTipoviClanarina());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Preuzmi sve fitnes usluge (za padajuće menije)
        /// </summary>
        [HttpGet("usluge")]
        public ActionResult<List<FitnesUsluga>> GetUsluge()
        {
            try
            {
                return Ok(_lookupService.GetAllUsluge());
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
