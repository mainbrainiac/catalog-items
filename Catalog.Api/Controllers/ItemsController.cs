using Catalog.Api.Dtos;
using Catalog.Api.Api.Entities;
using Catalog.Api.Api.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Api.Controllers
{
  [ApiController]
  [Route("items")]
  public class ItemsController :  ControllerBase
  {
    private readonly IItemsRepository _repository;
    private readonly ILogger<ItemsController> _logger;

    public ItemsController(IItemsRepository repository, ILogger<ItemsController> logger)
    {
      _repository = repository;
      _logger = logger;
    }
 
    [HttpGet]
    public async Task<IEnumerable<ItemDTO>> GetItemsAsync(string nameToMatch = null) 
    {
      var items = (await _repository.GetItemsAsync())
                  .Select(item => item.AsDTO());
      _logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {items.Count()} items");

      if (!string.IsNullOrWhiteSpace(nameToMatch))
      {
        items = items.Where(item => item.Name.Contains(nameToMatch, StringComparison.OrdinalIgnoreCase));
      }

      return items;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDTO>> GetItemAsync(Guid id) 
    {
      var item = await _repository.GetItemAsync(id);
      if (item is null) return NotFound();
      return Ok(item.AsDTO());
    }

    [HttpPost]
    public async Task<ActionResult<ItemDTO>> CreateItemAsync(CreateItemDTO itemDTO)
    {
      Item item = new() {
        Id = Guid.NewGuid(),
        Name = itemDTO.Name,
        Description = itemDTO.Description,
        Price = itemDTO.Price,
        CreatedDate = DateTimeOffset.UtcNow
      };
      await _repository.CreateItemAsync(item);
      return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDTO());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDTO itemDTO) 
    {
      var existing = await _repository.GetItemAsync(id);
      if (existing is null) return NotFound();
      existing.Name = itemDTO.Name;
      existing.Price = itemDTO.Price;
      await _repository.UpdateItemAsync(existing);
      return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItemAsync(Guid id)
    {
      var existing = await _repository.GetItemAsync(id);
      if (existing is null) return NotFound();
      await _repository.DeleteItemAsync(id);
      return NoContent();
    }
  }
}