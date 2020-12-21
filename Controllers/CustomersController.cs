using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sakila.Data;
using Sakila.Models;

namespace Sakila.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly SakilaDbContext _context;

        public CustomersController(SakilaDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        // POST: api/customers/5/rentFilm/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{customerId}/rentFilm/{filmId}")]
        public async Task<ActionResult<Customer>> RentFilm(int customerId, int filmId)
        {
            var customer = await _context.Customers
                .SingleOrDefaultAsync(c => c.CustomerId == customerId);

            if(customer == null)
            {
                return BadRequest("Customer does not exist!");
            }

            // get inventory for the filmId
            // include on Film to get the title
            // include on Rentals to check availability
            var inventory = await _context.Inventory
                .Include(i => i.Film)
                .Include(i => i.Rentals)
                .Where(i => i.FilmId == filmId)
                .ToListAsync();

            // check if any of them are available
            var availableInv = inventory.FirstOrDefault(i => i.Available);

            if (availableInv == null)
            {
                return BadRequest("Film not avabilable for renting!");
            }

            var rental = new Rental()
            {
                CustomerId = customerId,
                InventoryId = availableInv.InventoryId,
                RentalDate = DateTime.Now
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return Ok($"Customer {customer.FirstName} rented the movie {availableInv.Film.Title} at {rental.RentalDate}");
        }

        //    // check if movie exists in inventory
        //    var inventory = await _context.Inventory
        //        .Where(film => film.FilmId == filmToRent.FilmId)
        //        .Include(f => f.Rentals)
        //        .ToListAsync();

        //    // check inventory if the any are returned and available
        //    var availableFilm = inventory.FirstOrDefault(i => i.Rentals.Count == 0 || i.Rentals.Any(r => r.ReturnDate != null));

        //        if(availableFilm == null)
        //        {
        //            return NotFound();
        //}

        // POST: api/Customers/5/returnFilm/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{customerId}/returnFilm/{filmId}")]
        public async Task<ActionResult<Customer>> ReturnFilm(int customerId, int filmId)
        {
            // hämta kunden och kundens alla hyrningar.
            var customer = await _context.Customers
                .Include(c => c.Rentals)
                .ThenInclude(r => r.Inventory)
                .ThenInclude(i => i.Film)
                .SingleOrDefaultAsync(c => c.CustomerId == customerId);

            if(customer == null)
            {
                return BadRequest("Customer does not exist!");
            }

            if(customer.Rentals == null || customer.Rentals.Count == 0)
            {
                return BadRequest("Customer does not have any rentals!");
            }

            // kolla om kunden har hyrt filmen med detta id
            // har kunden hyrt två av samma film så plockas den första 
            // eftersom FirstOrDefault används.
            var rental = customer.Rentals.FirstOrDefault(r => r.Inventory.FilmId == filmId && !r.Returned);

            if(rental == null)
            {
                return BadRequest("Customer have not rented this movie.");
            }

            // har vi kommit hit så har kunden hyrt filmen och den återlämnas genom att sätta returnDate
            _context.Entry(rental).State = EntityState.Modified;
            rental.ReturnDate = DateTime.Now;
            
            await _context.SaveChangesAsync();

            return Ok($"Customer {customer.FirstName} return the movie {rental.Inventory.Film.Title} at {rental.RentalDate}");
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
