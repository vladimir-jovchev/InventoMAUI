using Invento.Infrastructure.Data;
using Invento.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BusinessesController : ControllerBase {
	private readonly ApplicationDbContext _context;

	public BusinessesController(ApplicationDbContext context) {
		_context = context;
	}

	// GET: api/Businesses
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Business>>> GetBusinesses() {
		return await _context.Businesses.ToListAsync();
	}

	// GET: api/Businesses/5
	[HttpGet("{id}")]
	public async Task<ActionResult<Business>> GetBusiness(int id) {
		var business = await _context.Businesses.FindAsync(id);

		if (business == null) return NotFound();

		return business;
	}

	// POST: api/Businesses
	[HttpPost]
	public async Task<ActionResult<Business>> CreateBusiness(Business business) {
		business.CreatedAt = DateTime.UtcNow;
		_context.Businesses.Add(business);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetBusiness), new { id = business.Id }, business);
	}

	// PUT: api/Businesses/5
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateBusiness(int id, Business business) {
		if (id != business.Id) return BadRequest();

		business.ModifiedAt = DateTime.UtcNow;
		_context.Entry(business).State = EntityState.Modified;

		try {
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException) {
			if (!BusinessExists(id)) return NotFound();

			throw;
		}

		return NoContent();
	}

	// DELETE: api/Businesses/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteBusiness(int id) {
		var business = await _context.Businesses.FindAsync(id);
		if (business == null) return NotFound();

		_context.Businesses.Remove(business);
		await _context.SaveChangesAsync();

		return NoContent();
	}

	private bool BusinessExists(int id) {
		return _context.Businesses.Any(e => e.Id == id);
	}
}