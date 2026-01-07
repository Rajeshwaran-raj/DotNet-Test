using Microsoft.AspNetCore.Mvc;
using Dotnet8MySqlCrud.Models;

namespace Dotnet8MySqlCrud.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // 🔥 In-memory database (static so it persists while app runs)
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 55000, Stock = 10, CreatedAt = DateTime.UtcNow },
        new Product { Id = 2, Name = "Mouse", Price = 500, Stock = 100, CreatedAt = DateTime.UtcNow },
        new Product { Id = 3, Name = "Keyboard", Price = 1200, Stock = 50, CreatedAt = DateTime.UtcNow }
    };

    private static int _nextId = 4;

    // GET: api/products
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
        => Ok(_products);

    // GET: api/products/{id}
    [HttpGet("{id:int}")]
    public ActionResult<Product> GetById(int id)
    {
        var item = _products.FirstOrDefault(p => p.Id == id);
        return item is null ? NotFound() : Ok(item);
    }

    // POST: api/products
    [HttpPost]
    public ActionResult<Product> Create([FromBody] Product dto)
    {
        dto.Id = _nextId++;
        dto.CreatedAt = DateTime.UtcNow;
        _products.Add(dto);

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    // PUT: api/products/{id}
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Product dto)
    {
        if (id != dto.Id)
            return BadRequest("ID mismatch.");

        var item = _products.FirstOrDefault(p => p.Id == id);
        if (item is null)
            return NotFound();

        item.Name = dto.Name;
        item.Price = dto.Price;
        item.Stock = dto.Stock;

        return NoContent();
    }

    // PATCH: api/products/{id}/stock?value=10
    [HttpPatch("{id:int}/stock")]
    public IActionResult UpdateStock(int id, [FromQuery] int value)
    {
        var item = _products.FirstOrDefault(p => p.Id == id);
        if (item is null)
            return NotFound();

        item.Stock = value;
        return Ok(item);
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var item = _products.FirstOrDefault(p => p.Id == id);
        if (item is null)
            return NotFound();

        _products.Remove(item);
        return NoContent();
    }
}
