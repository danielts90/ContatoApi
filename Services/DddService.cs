using ContatoApi.Models;

namespace ContatoApi.Services;

public class DddService : IDddService
{
     private readonly IHttpClientFactory _httpClient;
    private List<Ddd> _cache = new();
    public IEnumerable<Ddd> GetCachedDdds() => _cache;

    public DddService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<Ddd>> GetDdds()
    {
        var client = _httpClient.CreateClient("ddd");
        var response = await client.GetAsync("ddd");

        if(response.IsSuccessStatusCode)
        {
            var ddds = await response.Content.ReadFromJsonAsync<List<Ddd>>();
            if(ddds != null && ddds.Any())
                _cache = ddds;
        }
        return _cache;        
    }    

    public bool UpdateCache(EventTypes eventType, Ddd ddd)
    {
        switch(eventType)
        {
            case EventTypes.CREATE:
                _cache.Add(ddd);
                return true;
            case EventTypes.UPDATE:
                var updated = _cache.FirstOrDefault(o => o.Id == ddd.Id);
                if(updated is Ddd)
                {
                    _cache.Remove(updated);
                    _cache.Add(ddd);
                    return true;
                }
                return false;
            case EventTypes.DELETE:
                var deleted = _cache.FirstOrDefault(o => o.Id == ddd.Id);
                if(deleted is Ddd)
                {
                    _cache.Remove(deleted);
                    return true;
                }
                return false;
            default:
                return false;

        }
    }
}