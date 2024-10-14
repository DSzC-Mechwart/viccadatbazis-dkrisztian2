using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using ViccAdatbazis.Data;
using ViccAdatbazis.Models;

namespace ViccAdatbazis.Controllers
{
    [Route("api/[controller")]
    [ApiController]
    public class vicccontroller : ControllerBase
    {
        private readonly ViccDbContext _context;
        public vicccontroller(ViccDbContext context)
        {
            _context = context;
        }

        //Viccek lekérdezése
        [HttpGet]
        public async Task<ActionResult<List<Vicc>>> GetViccek() 
        {
            return await _context.Viccek.Where(x => x.Aktiv).ToListAsync();
        }

        //vicc lekérdezése

        [HttpGet("{id}")]
        public async Task<ActionResult<Vicc>> GetVicc(int id)
        {
            var vicc = await _context.Viccek.FindAsync(id);

            return vicc == null ? NotFound() : Ok(vicc);
        }

        //Új vicc feltöltése

        [HttpPost]
        
        public async Task<ActionResult> PostVicc(Vicc ujVicc)
        {
            _context.Viccek.Add(ujVicc);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /*
        public async Task<ActionResult<Vicc>> PostVicc(Vicc ujVicc)
        {
            _conte(Viccek.Add(ujVicc);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetVicc", new { id = ujVicc.Id }, ujVicc);
        }
        */

        //Vicc módosítása

        [HttpPut("{id}")]
        public async Task<ActionResult> PutVicc(int id, Vicc modositottVicc)
        {
            if (id != modositottVicc.Id)
            {
                return BadRequest();
            }
            _context.Entry(modositottVicc).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }

        //Vicc törlése

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVicc(int id)
        {
            var torlendo = _context.Viccek.Find(id);
            if (torlendo == null)
                return NotFound();
            if (torlendo.Aktiv)
            {
                torlendo.Aktiv = false;
                _context.Entry(torlendo).State = EntityState.Modified;
            }
                
            else
                _context.Viccek.Remove(torlendo);

            await _context.SaveChangesAsync();
            return Ok();
        }

        //Like

        [Route("{id}/Like")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> Like(int id)
        {
            var vicc = _context.Viccek.Find(id);
            if (vicc == null)
            {
                return NotFound();
            }

            vicc.Tetszik++;
            _context.Entry(vicc).State = EntityState.Modified;           

            await _context.SaveChangesAsync();
            return Ok();
        }

        //Dislike
    }
}
