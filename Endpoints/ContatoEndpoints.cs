using ContatoApi.Context;
using ContatoApi.Entities;
using Microsoft.EntityFrameworkCore;

public static class ContatoEndpoints
{
    public static void MapContatoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("/contato")
            .MapEndpoints()
            .WithTags("Contato");
    }

    public static async Task<IResult> HandleGetAll(ContatoDb db)
{
    return TypedResults.Ok(await db.Contatos.ToArrayAsync());
}
public static async Task<IResult> HandleGetById(int id, ContatoDb db)
{
    var contato = await db.Contatos.FindAsync(id);
    return  TypedResults.Ok(contato);
}
public static async Task<IResult> HandleCreate(Contato contato, ContatoDb db)
{
    db.Contatos.Add(contato);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/contato/{contato.Id}", contato);
}
public static async Task<IResult> HandleUpdate(int id, Contato inputContato, ContatoDb db)
{
    var contato = await db.Contatos.FindAsync(id);

    if (contato is null) return TypedResults.NotFound();

    contato = inputContato;
    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}
public static async Task<IResult> HandleDelete(int id, ContatoDb db)
{
    if (await db.Contatos.FindAsync(id) is Contato contato)
    {
        db.Contatos.Remove(contato);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NotFound();
}
private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
{
    group.MapGet("/", HandleGetAll);
    group.MapGet("/{id}", HandleGetById);
    group.MapPost("/", HandleCreate);
    group.MapPut("/{id}", HandleUpdate);
    group.MapDelete("/{id}", HandleDelete);
    return group;
 }
}