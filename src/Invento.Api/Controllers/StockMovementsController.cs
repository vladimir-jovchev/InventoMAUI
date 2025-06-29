using Invento.Infrastructure.Data;
using Invento.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockMovementsController : ControllerBase {
	private readonly ApplicationDbContext _context;

	public StockMovementsController(ApplicationDbContext context) {
		_context = context;
	}

	// GET: api/StockMovements
	[HttpGet]
	public async Task<ActionResult<IEnumerable<StockMovement>>> GetStockMovements() {
		return await _context.StockMovements
			.Include(s => s.Product)
			.Include(s => s.Business)
			.Include(s => s.User)
			.ToListAsync();
	}

	// GET: api/StockMovements/5
	[HttpGet("{id}")]
	public async Task<ActionResult<StockMovement>> GetStockMovement(int id) {
		var stockMovement = await _context.StockMovements
			.Include(s => s.Product)
			.Include(s => s.Business)
			.Include(s => s.User)
			.FirstOrDefaultAsync(s => s.Id == id);

		if (stockMovement == null) return NotFound();

		return stockMovement;
	}

	// GET: api/StockMovements/Product/5
	[HttpGet("Product/{productId}")]
	public async Task<ActionResult<IEnumerable<StockMovement>>> GetProductStockMovements(int productId) {
		return await _context.StockMovements
			.Include(s => s.Product)
			.Include(s => s.Business)
			.Include(s => s.User)
			.Where(s => s.ProductId == productId)
			.ToListAsync();
	}

	// POST: api/StockMovements
	[HttpPost]
	public async Task<ActionResult<StockMovement>> CreateStockMovement(StockMovement stockMovement) {
		stockMovement.MovementDate = DateTime.UtcNow;
		stockMovement.TotalCost = stockMovement.Quantity * stockMovement.UnitCost;

		_context.StockMovements.Add(stockMovement);

		// Update inventory level
		var inventoryLevel = await _context.InventoryLevels
			.FirstOrDefaultAsync(i =>
				i.ProductId == stockMovement.ProductId && i.BusinessId == stockMovement.BusinessId);

		if (inventoryLevel == null) {
			// Create new inventory level if it doesn't exist
			inventoryLevel = new InventoryLevel {
				ProductId = stockMovement.ProductId,
				BusinessId = stockMovement.BusinessId ?? 0,
				CurrentStock = 0,
				MinimumStock = 0,
				ReorderPoint = 0,
				MaximumStock = 1000,
				LastUpdated = DateTime.UtcNow,
				AverageCost = stockMovement.UnitCost,
				Status = InventoryStatus.InStock
			};
			_context.InventoryLevels.Add(inventoryLevel);
		}

		// Update stock quantity based on movement type
		switch (stockMovement.Type) {
			case MovementType.Purchase:
			case MovementType.Return:
				inventoryLevel.CurrentStock += stockMovement.Quantity;
				break;
			case MovementType.Sale:
			case MovementType.WriteOff:
				inventoryLevel.CurrentStock -= stockMovement.Quantity;
				break;
			case MovementType.Transfer:
				// For transfers, you'd need to handle both source and destination
				inventoryLevel.CurrentStock -= stockMovement.Quantity;
				break;
			case MovementType.StockCount:
				inventoryLevel.CurrentStock = stockMovement.Quantity; // Set to counted value
				break;
			case MovementType.Adjustment:
				// For adjustments, we assume the quantity already represents the net change
				inventoryLevel.CurrentStock += stockMovement.Quantity;
				break;
		}

		inventoryLevel.LastUpdated = DateTime.UtcNow;

		// Update inventory status
		if (inventoryLevel.CurrentStock <= 0)
			inventoryLevel.Status = InventoryStatus.OutOfStock;
		else if (inventoryLevel.CurrentStock < inventoryLevel.ReorderPoint)
			inventoryLevel.Status = InventoryStatus.LowStock;
		else
			inventoryLevel.Status = InventoryStatus.InStock;

		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetStockMovement), new { id = stockMovement.Id }, stockMovement);
	}

	// DELETE: api/StockMovements/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteStockMovement(int id) {
		var stockMovement = await _context.StockMovements.FindAsync(id);
		if (stockMovement == null) return NotFound();

		_context.StockMovements.Remove(stockMovement);
		await _context.SaveChangesAsync();

		return NoContent();
	}
}