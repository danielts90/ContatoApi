using ContatoApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContatoApi.Context;

public class ContatoDb : DbContext
{
    public ContatoDb(DbContextOptions<ContatoDb> options) 
    : base(options){}

    public DbSet<Contato> Contatos => Set<Contato>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Contato>()
            .ToTable("contato") 
            .HasKey(c => c.Id);

        builder.Entity<Contato>()
        .Property(c => c.Id)
        .ValueGeneratedOnAdd();

        builder.Entity<Contato>()
            .Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(100); 
        
        builder.Entity<Contato>()
            .Property(c => c.Email)
            .IsRequired();
        
        builder.Entity<Contato>()
            .Property(c => c.Telefone)
            .IsRequired();

        builder.Entity<Contato>()
            .Property(c => c.DddId)
            .IsRequired();
    }


}

