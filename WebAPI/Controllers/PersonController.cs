using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassLibrary.Data;
using ClassLibrary.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly EFCoreDBContext dbContext;
        private readonly IHubContext<ChatHub> hubContext;

        public PersonController(EFCoreDBContext dbContext, IHubContext<ChatHub> hubContext)
        {
            this.dbContext = dbContext;
            this.hubContext = hubContext;
        }

        // GET: Person/GetPersons
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return await dbContext.Persons.ToListAsync();
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersonsByName(string? filterName,int currentPage)
        {
            if (string.IsNullOrWhiteSpace(filterName))
            {
                return await dbContext.Persons.Skip((currentPage-1)*5).Take(5).ToListAsync();
            }
            else
            {
                return await dbContext.Persons.Where(p => p.FirstName.Contains(filterName)).ToListAsync();
            }
            
        }
        // GET: Person/GetPersonByNumber
        [HttpGet]
        public async Task<ActionResult<Person>> GetPersonById(long id)
        {
            var person = await dbContext.Persons.FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }
        // POST: Person/AddPerson
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]

        public async Task<ActionResult> AddPerson(Person person)
        {
            var item = new Person
            {
                Number = person.Number,
                FirstName = person.FirstName,
            };
            dbContext.Persons.Add(item);
            await dbContext.SaveChangesAsync();
            await hubContext.Clients.All.SendAsync("NoteMessage", "Update");//变更后发送信息给在线用户重新加载List页面，下同。
            return CreatedAtAction(nameof(GetPersonById), new { Id = person.Id }, person);
        }

        // DELETE: Person/DeletePerson
        [HttpDelete]
        [Authorize(Roles = "AdminRole")]
        public async Task<IActionResult> DeletePerson(long id)
        {
            var person = await dbContext.Persons.FirstOrDefaultAsync(p => p.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            dbContext.Persons.Remove(person);
            await dbContext.SaveChangesAsync();
            await hubContext.Clients.All.SendAsync("NoteMessage", "Update");
            return NoContent();
        }

        // PUT:Person/EditPerson
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<Person>> EditPerson(long id, Person person)
        {
            var newPerson = await dbContext.Persons.FirstOrDefaultAsync(p => p.Id == id);

            if (id != newPerson.Id)
            {
                return BadRequest();
            }
            newPerson.Number = person.Number;
            newPerson.FirstName = person.FirstName;
            dbContext.Persons.Update(newPerson);
            try
            {
                await dbContext.SaveChangesAsync();
                await hubContext.Clients.All.SendAsync("NoteMessage", "Update");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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
        private bool PersonExists(long id)
        {
            return dbContext.Persons.Any(e => e.Id == id);
        }
    }
}
