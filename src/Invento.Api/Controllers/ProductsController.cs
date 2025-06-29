using Invento.Infrastructure.Data;
using Invento.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase {
	private readonly ApplicationDbContext _context;

	public ProductsController(ApplicationDbContext context) {
		_context = context;
	}

	// GET: api/Products
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Product>>> GetProducts() {
		return await _context.Products
			.Include(p => p.Category)
			.Include(p => p.Business)
			.Include(p => p.Supplier)
			.ToListAsync();
	}

	// GET: api/Products/5
	[HttpGet("{id}")]
	public async Task<ActionResult<Product>> GetProduct(int id) {
		var product = await _context.Products
			.Include(p => p.Category)
			.Include(p => p.Business)
			.Include(p => p.Supplier)
			.FirstOrDefaultAsync(p => p.Id == id);

		if (product == null) return NotFound();

		return product;
	}

	// GET: api/Products/Search?term=keyword
	[HttpGet("Search")]
	public async Task<ActionResult<IEnumerable<Product>>> SearchProducts(string term) {
		if (string.IsNullOrEmpty(term)) return await _context.Products.Take(10).ToListAsync();

		return await _context.Products
			.Where(p => p.Name.Contains(term) ||
						p.SKU.Contains(term) ||
						p.Barcode.Contains(term) ||
						p.Description.Contains(term))
			.ToListAsync();
	}

	// POST: api/Products
	[HttpPost]
	public async Task<ActionResult<Product>> CreateProduct(Product product) {
		product.CreatedAt = DateTime.UtcNow;
		_context.Products.Add(product);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
	}

	// PUT: api/Products/5
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateProduct(int id, Product product) {
		if (id != product.Id) return BadRequest();

		product.ModifiedAt = DateTime.UtcNow;
		_context.Entry(product).State = EntityState.Modified;

		try {
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException) {
			if (!ProductExists(id)) return NotFound();

			throw;
		}

		return NoContent();
	}

	// DELETE: api/Products/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteProduct(int id) {
		var product = await _context.Products.FindAsync(id);
		if (product == null) return NotFound();

		_context.Products.Remove(product);
		await _context.SaveChangesAsync();

		return NoContent();
	}

	private bool ProductExists(int id) {
		return _context.Products.Any(e => e.Id == id);
	}
}