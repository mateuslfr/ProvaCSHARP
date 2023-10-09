using Prova.Models;
using Microsoft.EntityFrameworkCore;


namespace Prova.Data
{
    public class AppDataContext : DbContext
    {
          public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
    {

    }

    //Classes que vão se tornar tabelas no banco de dados
    public DbSet<Funcionario> Funcionarios { get; set; } = null!;
    public DbSet<Folha> Folhas { get; set; } = null!;


       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlite("DataSource=folha.db");
}

    }
}