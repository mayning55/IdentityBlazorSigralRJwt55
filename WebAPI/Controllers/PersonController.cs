﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassLibrary.Data;
using ClassLibrary.Settings;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly EFCoreDBContext _context;

        public PersonController(EFCoreDBContext context)
        {
            _context = context;
        }

        // GET: Person/GetPersons
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return await _context.Persons.ToListAsync();
        }
        // GET: Person/GetPersonByNumber
        [HttpGet]
        public async Task<ActionResult<Person>> GetPersonById(long id)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);

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
            _context.Persons.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPersonById),new { Id = person.Id }, person);
        }

        // DELETE: Person/DeletePerson
        [HttpDelete]
        [Authorize(Roles = "AdminRole")]
        public async Task<IActionResult> DeletePerson(long id)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT:Person/EditPerson
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<Person>> EditPerson(long id, Person person)
        {
            var newPerson= await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);

            if (id != newPerson.Id)
            {
                return BadRequest();
            }
            newPerson.Number = person.Number;
            newPerson.FirstName = person.FirstName;
            _context.Persons.Update(newPerson);
            try
            {
                await _context.SaveChangesAsync();
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
            return _context.Persons.Any(e => e.Id == id);
        }
    }
}