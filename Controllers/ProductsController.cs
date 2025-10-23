using Dotnet8MySqlCrud.Data;
using Dotnet8MySqlCrud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dotnet8MySqlCrud.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        => Ok(await db.Products.AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var item = await db.Products.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create([FromBody] Product dto)
    {
        dto.Id = 0;
        dto.CreatedAt = DateTime.UtcNow;
        db.Products.Add(dto);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");
        var exists = await db.Products.AnyAsync(p => p.Id == id);
        if (!exists) return NotFound();

        db.Entry(dto).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id:int}/stock")]
    public async Task<IActionResult> UpdateStock(int id, [FromQuery] int value)
    {
        var item = await db.Products.FindAsync(id);
        if (item is null) return NotFound();
        item.Stock = value;
        await db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await db.Products.FindAsync(id);
        if (item is null) return NotFound();
        db.Products.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
