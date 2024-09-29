namespace ContatoApi.Entities;

public class Contato
{
    public int? Id {get; set;}
    public string? Nome { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public long? DddId { get; set; }
}