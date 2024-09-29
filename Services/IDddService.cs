using ContatoApi.Models;

namespace ContatoApi.Services;

public interface IDddService
{
    Task<List<Ddd>> GetDdds();
    bool UpdateCache(EventTypes eventType, Ddd ddd);
    IEnumerable<Ddd> GetCachedDdds();
}