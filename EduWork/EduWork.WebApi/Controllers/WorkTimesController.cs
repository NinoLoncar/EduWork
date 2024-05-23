using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduWork.Data;
using EduWork.Domain.Entitites;
using Microsoft.Identity.Web.Resource;
using Microsoft.AspNetCore.Authorization;

namespace EduWork.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class WorkTimesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkTimesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/WorkTimes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkTime>>> GetWorkTimes()
        {
            return await _context.WorkTimes.ToListAsync();
        }

        // GET: api/WorkTimes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkTime>> GetWorkTime(Guid id)
        {
            var workTime = await _context.WorkTimes.FindAsync(id);

            if (workTime == null)
            {
                return NotFound();
            }

            return workTime;
        }

        // PUT: api/WorkTimes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkTime(Guid id, WorkTime workTime)
        {
            if (id != workTime.Id)
            {
                return BadRequest();
            }

            _context.Entry(workTime).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkTimeExists(id))
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

        // POST: api/WorkTimes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkTime>> PostWorkTime(WorkTime workTime)
        {
            _context.WorkTimes.Add(workTime);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkTime", new { id = workTime.Id }, workTime);
        }

        // DELETE: api/WorkTimes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkTime(Guid id)
        {
            var workTime = await _context.WorkTimes.FindAsync(id);
            if (workTime == null)
            {
                return NotFound();
            }

            _context.WorkTimes.Remove(workTime);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkTimeExists(Guid id)
        {
            return _context.WorkTimes.Any(e => e.Id == id);
        }
    }
}
