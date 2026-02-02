using FilmesAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FilmesAPI.Data;

public class AppDbContext : IdentityDbContext<Usuario>
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts)
    {
            
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //SOFT DELETE
        builder.Entity<Filme>().HasQueryFilter(filme => filme.DataExclusao == null);
        builder.Entity<Cinema>().HasQueryFilter(cinema => cinema.DataExclusao == null);
        builder.Entity<Endereco>().HasQueryFilter(endereco => endereco.DataExclusao == null);
        builder.Entity<Sessao>().HasQueryFilter(sessao => sessao.DataExclusao == null);

        // protege endereço e cinema de serem deletados em cascata
        builder.Entity<Endereco>()
            .HasOne(endereco => endereco.Cinema)
            .WithOne(cinema => cinema.Endereco)
            .OnDelete(DeleteBehavior.Restrict);


        // protege as sessões de sumirem se alguém tentar apagar o cinema
        builder.Entity<Sessao>()
            .HasOne(sessao => sessao.Cinema)
            .WithMany(cinema => cinema.Sessoes)
            .HasForeignKey(sessao => sessao.CinemaId)
            .OnDelete(DeleteBehavior.Restrict);

        // protege as sessões de sumirem se alguém tentar apagar o filme
        builder.Entity<Sessao>()
            .HasOne(sessao => sessao.Filme)
            .WithMany(filme => filme.Sessoes)
            .HasForeignKey(sessao => sessao.FilmeId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<Filme> Filmes { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet <Endereco> Enderecos {  get; set; }
    public DbSet<Sessao> Sessoes { get; set; }
}
