using Catalog.Api.Api.Entities;

namespace Catalog.Api.Api.Repositories.Contracts
{
    public interface IItemsRepository
    {
      Task<Item> GetItemAsync(Guid id);
      Task<IEnumerable<Item>> GetItemsAsync();
      Task CreateItemAsync(Item item);
      Task UpdateItemAsync(Item item);
      Task DeleteItemAsync(Guid id);
    }
}
