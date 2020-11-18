using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Heating.Core.Schedule;
using Heating.Core.Data;

namespace Heating.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly DataContext _context;

        public ScheduleController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Schedule
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduledEvent>>> GetScheduledEvents()
        {
            return await _context.ScheduledEvents.ToListAsync();
        }

        // GET: api/Schedule/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduledEvent>> GetScheduledEvent(int id)
        {
            var scheduledEvent = await _context.ScheduledEvents.FindAsync(id);

            if (scheduledEvent == null)
            {
                return NotFound();
            }

            return scheduledEvent;
        }

        // PUT: api/Schedule/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScheduledEvent(int id, ScheduledEvent scheduledEvent)
        {
            if (id != scheduledEvent.EventID)
            {
                return BadRequest();
            }

            _context.Entry(scheduledEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScheduledEventExists(id))
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

        // POST: api/Schedule
        [HttpPost]
        public async Task<ActionResult<ScheduledEvent>> PostScheduledEvent(ScheduledEvent scheduledEvent)
        {
            _context.ScheduledEvents.Add(scheduledEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScheduledEvent", new { id = scheduledEvent.EventID }, scheduledEvent);
        }

        // DELETE: api/Schedule/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ScheduledEvent>> DeleteScheduledEvent(int id)
        {
            var scheduledEvent = await _context.ScheduledEvents.FindAsync(id);
            if (scheduledEvent == null)
            {
                return NotFound();
            }

            _context.ScheduledEvents.Remove(scheduledEvent);
            await _context.SaveChangesAsync();

            return scheduledEvent;
        }

        private bool ScheduledEventExists(int id)
        {
            return _context.ScheduledEvents.Any(e => e.EventID == id);
        }
    }
}
