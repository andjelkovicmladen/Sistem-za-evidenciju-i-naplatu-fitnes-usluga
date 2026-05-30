using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domen;
using WebAPI.Services;
using Zajednicki;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClanoviController : ControllerBase
    {
        private readonly IClanService _clanService;

        public ClanoviController(IClanService clanService)
        {
            _clanService = clanService;
        }

        /// <summary>
        /// Preuzmi sve članove
        /// </summary>
        [HttpGet]
        public ActionResult<List<Clan>> GetAll()
        {
            try
            {
                var clani = _clanService.GetAllClans();
                return Ok(clani);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Preuzmi člana po ID-u
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Clan> GetById(int id)
        {
            try
            {
                var clan = _clanService.GetClanById(id);
                if (clan == null)
                    return NotFound(new { message = "Član nije pronađen." });

                return Ok(clan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Pretraži članove
        /// </summary>
        [HttpPost("search")]
        public ActionResult<List<Clan>> Search([FromBody] ClanSearchParametri parametri)
        {
            try
            {
                var rezultati = _clanService.SearchClans(parametri);
                return Ok(rezultati);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Kreiraj novog člana
        /// </summary>
        [HttpPost]
        public IActionResult Create([FromBody] Clan clan)
        {
            try
            {
                if (clan == null)
                    return BadRequest(new { message = "Podaci člana su obavezni." });

                _clanService.CreateClan(clan);
                return CreatedAtAction(nameof(GetById), new { id = clan.IdClan }, clan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Ažuriraj člana
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Clan clan)
        {
            try
            {
                if (clan == null || clan.IdClan != id)
                    return BadRequest(new { message = "Nevaljani podaci." });

                var postojeci = _clanService.GetClanById(id);
                if (postojeci == null)
                    return NotFound(new { message = "Član nije pronađen." });

                _clanService.UpdateClan(clan);
                return Ok(new { message = "Član je uspešno ažuriran." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obriši člana
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var clan = _clanService.GetClanById(id);
                if (clan == null)
                    return NotFound(new { message = "Član nije pronađen." });

                _clanService.DeleteClan(id);
                return Ok(new { message = "Član je uspešno obrisan." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
