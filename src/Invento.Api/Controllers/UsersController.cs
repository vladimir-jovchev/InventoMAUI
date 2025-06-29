using Invento.Infrastructure.Data;
using Invento.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase {
	private readonly ApplicationDbContext _context;

	public UsersController(ApplicationDbContext context) {
		_context = context;
	}

	// GET: api/Users
	[HttpGet]
	public async Task<ActionResult<IEnumerable<User>>> GetUsers() {
		return await _context.Users
			.Include(u => u.Business)
			.ToListAsync();
	}

	// GET: api/Users/5
	[HttpGet("{id}")]
	public async Task<ActionResult<User>> GetUser(int id) {
		var user = await _context.Users
			.Include(u => u.Business)
			.FirstOrDefaultAsync(u => u.Id == id);

		if (user == null) return NotFound();

		return user;
	}

	// POST: api/Users
	[HttpPost]
	public async Task<ActionResult<User>> CreateUser(User user) {
		user.CreatedAt = DateTime.UtcNow;
		_context.Users.Add(user);
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
	}

	// PUT: api/Users/5
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateUser(int id, User user) {
		if (id != user.Id) return BadRequest();

		_context.Entry(user).State = EntityState.Modified;

		try {
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException) {
			if (!UserExists(id)) return NotFound();

			throw;
		}

		return NoContent();
	}

	// DELETE: api/Users/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteUser(int id) {
		var user = await _context.Users.FindAsync(id);
		if (user == null) return NotFound();

		_context.Users.Remove(user);
		await _context.SaveChangesAsync();

		return NoContent();
	}

	// POST: api/Users/Login
	[HttpPost("Login")]
	public async Task<ActionResult<User>> Login(LoginRequest loginRequest) {
		var user = await _context.Users.FirstOrDefaultAsync(u =>
				u.Username == loginRequest.Username &&
				u.PasswordHash == loginRequest.Password // Note: In production, use proper password hashing!
		);

		if (user == null) return Unauthorized();

		user.LastLoginDate = DateTime.UtcNow;
		await _context.SaveChangesAsync();

		return user;
	}

	private bool UserExists(int id) {
		return _context.Users.Any(e => e.Id == id);
	}
}

// Simple DTO for login requests
public class LoginRequest {
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}