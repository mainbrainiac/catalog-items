using Catalog.Api.Api.Entities;
using Catalog.Api.Api.Repositories.Contracts;

namespace Catalog.Api.Api.Repositories
{
  public class InMemItemsRepository : IItemsRepository
  {
    private readonly List<Item> items = new() 
    {
      new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 10, CreatedDate = DateTimeOffset.UtcNow },
      new Item { Id = Guid.NewGuid(), Name = "Sword", Price = 20, CreatedDate = DateTimeOffset.UtcNow },
      new Item { Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 30, CreatedDate = DateTimeOffset.UtcNow }
    };

    public async Task<IEnumerable<Item>> GetItemsAsync() {
      return await Task.FromResult(items);
    } 

    public async Task<Item> GetItemAsync(Guid id) { 
      var item = items.FirstOrDefault(item => item.Id == id);
      return await Task.FromResult(item);
    }

    public async Task CreateItemAsync(Item item)
    {
      items.Add(item);
      await Task.CompletedTask;
    }
    public async Task UpdateItemAsync(Item item)
    {
      var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
      items[index] = item;
      await Task.CompletedTask;
    }

    public async Task DeleteItemAsync(Guid id)
    {
      var index = items.FindIndex(existingItem => existingItem.Id == id);
      items.RemoveAt(index);
      await Task.CompletedTask;
    }
  }
}