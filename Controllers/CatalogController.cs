using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microservico.Data;
using Microservico.Models;

namespace Microservico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext _context;

        public CatalogController(CatalogContext context)
        {
            _context = context;
        }

        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<CatalogItem>>> GetItems()
        {
            return await _context.CatalogItems.ToListAsync();
        }

        [HttpGet("items/{id}")]
        public async Task<ActionResult<CatalogItem>> GetItemById(int id)
        {
            var item = await _context.CatalogItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost("items")]
        public async Task<ActionResult<CatalogItem>> CreateItem(CatalogItem item)
        {
            _context.CatalogItems.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }

        [HttpPut("items/{id}")]
        public async Task<IActionResult> UpdateItem(int id, CatalogItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }
            _context.Entry(item).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.CatalogItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            _context.CatalogItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ItemExists(int id)
        {
            return _context.CatalogItems.Any(e => e.Id == id);
        }
    }
    }
