using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Heating.Core;
using Heating.Core.Data;

namespace Heating.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelaysController : ControllerBase
    {
        private readonly DataContext _context;

        public RelaysController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Relays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Relay>>> GetRelays()
        {
            return await _context.Relays.ToListAsync();
        }

        // GET: api/Relays/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Relay>> GetRelay(int id)
        {
            var relay = await _context.Relays.FindAsync(id);

            if (relay == null)
            {
                return NotFound();
            }

            return relay;
        }

        // GET: api/Relays/5
        [HttpGet("switch/{id}")]
        public async Task<ActionResult<Relay>> Switch(int id, bool value)
        {
            var relay = await _context.Relays.FindAsync(id);

            if (relay == null)
            {
                return NotFound();
            }

            await relay.SwitchAsync(value);

            return relay;
        }


        // PUT: api/Relays/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRelay(int id, Relay relay)
        {
            if (id != relay.ID)
            {
                return BadRequest();
            }

            _context.Entry(relay).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RelayExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Relays
        [HttpPost]
        public async Task<ActionResult<Relay>> PostRelay(Relay relay)
        {
            _context.Relays.Add(relay);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRelay", new { id = relay.ID }, relay);
        }

        // DELETE: api/Relays/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Relay>> DeleteRelay(int id)
        {
            var relay = await _context.Relays.FindAsync(id);
            if (relay == null)
            {
                return NotFound();
            }

            _context.Relays.Remove(relay);
            await _context.SaveChangesAsync();

            return relay;
        }

        private bool RelayExists(int id)
        {
            return _context.Relays.Any(e => e.ID == id);
        }
    }
}
