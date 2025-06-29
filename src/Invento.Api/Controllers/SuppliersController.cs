using Invento.Infrastructure.Data;
using Invento.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase {
	private readonly ApplicationDbContext _context;

	public SuppliersController(ApplicationDbContext context) {
		_context = context;
	}

	// GET: api/Suppliers
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers() {
		return await _context.Suppliers.ToListAsync();
	}

	// GET: api/Suppliers/5
	[HttpGet("{id}")]
	public async Task<ActionResult<Supplier>> GetSupplier(int id) {
		var supplier = await _context.Suppliers
			.Include(s => s.Products)
			.FirstOrDefaultAsync(s => s.Id == id);

		if (supplier == null) return NotFound();

		return supplier;
	}

	// POST: api/Suppliers
	[HttpPost]
	public async Task<ActionResult<Supplier>> CreateSupplier(Supplier supplier) {
		supplier.CreatedAt = DateTime.UtcNow;
		supplier.IsActive = true;
		_context.Suppliers.Add(supplier);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetSupplier), new { id = supplier.Id }, supplier);
	}

	// PUT: api/Suppliers/5
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateSupplier(int id, Supplier supplier) {
		if (id != supplier.Id) return BadRequest();

		supplier.ModifiedAt = DateTime.UtcNow;
		_context.Entry(supplier).State = EntityState.Modified;

		try {
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException) {
			if (!SupplierExists(id)) return NotFound();

			throw;
		}

		return NoContent();
	}

	// DELETE: api/Suppliers/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteSupplier(int id) {
		var supplier = await _context.Suppliers.FindAsync(id);
		if (supplier == null) return NotFound();

		// Soft delete - just mark as inactive
		supplier.IsActive = false;
		supplier.ModifiedAt = DateTime.UtcNow;
		await _context.SaveChangesAsync();

		return NoContent();
	}

	private bool SupplierExists(int id) {
		return _context.Suppliers.Any(e => e.Id == id);
	}
}