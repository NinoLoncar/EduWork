using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduWork.Data;
using Microsoft.Identity.Web.Resource;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using EduWork.Common.DTOs;
using EduWork.Data.Entitites;
using System.Security.Claims;

namespace EduWork.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class WorkTimesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WorkTimesController(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/WorkTimes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkTimeDTO>>> GetWorkTimes()
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == "preferred_username")?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Email is missing.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found.");

            }
            var workTimes = await _context.WorkTimes.Where(w=>w.UserId==user.Id ).OrderBy(w=>w.EndTime).ToListAsync();
            var workTimesDto = _mapper.Map<IEnumerable<WorkTimeDTO>>(workTimes);
            return Ok(workTimesDto);
        }

        // POST: api/WorkTimes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkTime>> PostWorkTime(WorkTimeDTO workTimeDto)
        {

            var email = _httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == "preferred_username")?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("Email is missing.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found.");

            }

            var workTime = _mapper.Map<WorkTime>(workTimeDto);
            workTime.UserId = user.Id;
            _context.WorkTimes.Add(workTime);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetWorkTime", new { id = workTime.Id }, workTime);
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
