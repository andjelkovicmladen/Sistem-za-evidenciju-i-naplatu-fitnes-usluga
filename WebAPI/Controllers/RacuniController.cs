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
    public class RacuniController : ControllerBase
    {
        private readonly IRacunService _racunService;

        public RacuniController(IRacunService racunService)
        {
            _racunService = racunService;
        }

        /// <summary>
        /// Preuzmi sve račune
        /// </summary>
        [HttpGet]
        public ActionResult<List<Racun>> GetAll()
        {
            try
            {
                var racuni = _racunService.GetAllRacuns();
                return Ok(racuni);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Preuzmi račun po ID-u
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Racun> GetById(int id)
        {
            try
            {
                var racun = _racunService.GetRacunById(id);
                if (racun == null)
                    return NotFound(new { message = "Račun nije pronađen." });

                return Ok(racun);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Preuzmi račun sa stavkama
        /// </summary>
        [HttpGet("{id}/stavke")]
        public ActionResult<Racun> GetWithStavke(int id)
        {
            try
            {
                var racun = _racunService.GetRacunWithStavke(id);
                if (racun == null)
                    return NotFound(new { message = "Račun nije pronađen." });

                return Ok(racun);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Pretraži račune
        /// </summary>
        [HttpPost("search")]
        public ActionResult<List<Racun>> Search([FromBody] RacunSearchParametri parametri)
        {
            try
            {
                var rezultati = _racunService.SearchRacuns(parametri);
                return Ok(rezultati);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Kreiraj novi račun
        /// </summary>
        [HttpPost]
        public IActionResult Create([FromBody] Racun racun)
        {
            try
            {
                if (racun == null)
                    return BadRequest(new { message = "Podaci računa su obavezni." });

                _racunService.CreateRacun(racun);
                return CreatedAtAction(nameof(GetById), new { id = racun.IdRacun }, racun);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Ažuriraj račun
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Racun racun)
        {
            try
            {
                if (racun == null || racun.IdRacun != id)
                    return BadRequest(new { message = "Nevaljani podaci." });

                var postojeci = _racunService.GetRacunById(id);
                if (postojeci == null)
                    return NotFound(new { message = "Račun nije pronađen." });

                _racunService.UpdateRacun(racun);
                return Ok(new { message = "Račun je uspešno ažuriran." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
