using Invento.Infrastructure.Data;
using Invento.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryLevelsController : ControllerBase {
	private readonly ApplicationDbContext _context;

	public InventoryLevelsController(ApplicationDbContext context) {
		_context = context;
	}

	// GET: api/InventoryLevels
	[HttpGet]
	public async Task<ActionResult<IEnumerable<InventoryLevel>>> GetInventoryLevels() {
		return await _context.InventoryLevels
			.Include(i => i.Product)
			.Include(i => i.Business)
			.ToListAsync();
	}

	// GET: api/InventoryLevels/5
	[HttpGet("{id}")]
	public async Task<ActionResult<InventoryLevel>> GetInventoryLevel(int id) {
		var inventoryLevel = await _context.InventoryLevels
			.Include(i => i.Product)
			.Include(i => i.Business)
			.FirstOrDefaultAsync(i => i.Id == id);

		if (inventoryLevel == null) return NotFound();

		return inventoryLevel;
	}

	// GET: api/InventoryLevels/Product/5
	[HttpGet("Product/{productId}")]
	public async Task<ActionResult<IEnumerable<InventoryLevel>>> GetProductInventory(int productId) {
		return await _context.InventoryLevels
			.Include(i => i.Business)
			.Where(i => i.ProductId == productId)
			.ToListAsync();
	}

	// GET: api/InventoryLevels/Business/5
	[HttpGet("Business/{businessId}")]
	public async Task<ActionResult<IEnumerable<InventoryLevel>>> GetBusinessInventory(int businessId) {
		return await _context.InventoryLevels
			.Include(i => i.Product)
			.Where(i => i.BusinessId == businessId)
			.ToListAsync();
	}

	// GET: api/InventoryLevels/LowStock
	[HttpGet("LowStock")]
	public async Task<ActionResult<IEnumerable<InventoryLevel>>> GetLowStockItems() {
		return await _context.InventoryLevels
			.Include(i => i.Product)
			.Include(i => i.Business)
			.Where(i => i.CurrentStock <= i.ReorderPoint && i.Status != InventoryStatus.Discontinued)
			.ToListAsync();
	}

	// PUT: api/InventoryLevels/5
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateInventoryLevel(int id, InventoryLevel inventoryLevel) {
		if (id != inventoryLevel.Id) return BadRequest();

		inventoryLevel.LastUpdated = DateTime.UtcNow;
		_context.Entry(inventoryLevel).State = EntityState.Modified;

		try {
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException) {
			if (!InventoryLevelExists(id)) return NotFound();

			throw;
		}

		return NoContent();
	}

	// POST: api/InventoryLevels/Adjust
	[HttpPost("Adjust")]
	public async Task<ActionResult<StockMovement>> AdjustInventory(InventoryAdjustment adjustment) {
		var inventoryLevel = await _context.InventoryLevels.FindAsync(adjustment.InventoryLevelId);

		if (inventoryLevel == null) return NotFound("Inventory level not found");

		// Create stock movement record
		var stockMovement = new StockMovement {
			ProductId = inventoryLevel.ProductId,
			BusinessId = inventoryLevel.BusinessId,
			Quantity = adjustment.AdjustmentQuantity,
			MovementDate = DateTime.UtcNow,
			Type = MovementType.Adjustment,
			Notes = adjustment.Notes,
			UserId = adjustment.UserId,
			UnitCost = adjustment.UnitCost ?? inventoryLevel.AverageCost,
			TotalCost = adjustment.AdjustmentQuantity * (adjustment.UnitCost ?? inventoryLevel.AverageCost),
			ReferenceNumber = $"ADJ-{DateTime.UtcNow:yyyyMMdd-HHmmss}"
		};

		_context.StockMovements.Add(stockMovement);

		// Update inventory level
		inventoryLevel.CurrentStock += adjustment.AdjustmentQuantity;
		inventoryLevel.LastUpdated = DateTime.UtcNow;

		// Update status based on new stock level
		if (inventoryLevel.CurrentStock <= 0)
			inventoryLevel.Status = InventoryStatus.OutOfStock;
		else if (inventoryLevel.CurrentStock < inventoryLevel.ReorderPoint)
			inventoryLevel.Status = InventoryStatus.LowStock;
		else
			inventoryLevel.Status = InventoryStatus.InStock;

		await _context.SaveChangesAsync();

		return CreatedAtAction(
			"GetStockMovement",
			"StockMovements",
			new { id = stockMovement.Id },
			stockMovement
		);
	}

	private bool InventoryLevelExists(int id) {
		return _context.InventoryLevels.Any(e => e.Id == id);
	}
}

// DTO for inventory adjustments
public class InventoryAdjustment {
	public int InventoryLevelId { get; set; }
	public decimal AdjustmentQuantity { get; set; }
	public string Notes { get; set; } = string.Empty;
	public int? UserId { get; set; }
	public decimal? UnitCost { get; set; }
}