namespace ContatoApi.Services;

public class DddHostedService : IHostedService
{
    private readonly IDddService _dddService;

    public DddHostedService(IDddService dddService)
    {
        _dddService = dddService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dddService.GetDdds();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Não foi possível obter a lista de Ddds. " + ex.Message);
        }
        
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
