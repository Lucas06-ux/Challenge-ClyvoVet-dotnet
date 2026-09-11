using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using SuperVet.Data;
using SuperVet.Models;
using Xunit;

namespace SuperVet.Integrations.Tests.FactoryFixture
{
    // Fixture que substitui o DbContext real por InMemory e popula dados de teste.
    public class ApiFactoryFixture : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Garantir que a aplicação rode em um ambiente de teste para evitar registrar providers de produção
            builder.UseEnvironment("IntegrationTests");

            builder.ConfigureServices(services =>
            {
                // Remove todas as possíveis inscrições do AppDbContext/DbContextOptions (Oracle)
                var descriptors = services.Where(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>) || d.ServiceType == typeof(AppDbContext)).ToList();
                foreach (var d in descriptors) services.Remove(d);

                // Registra AppDbContext em memória para testes de integração
                services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("IntegrationTestDb"));

                // Constrói o service provider e resolve o AppDbContext registrado para popular o banco
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Garante banco limpo e criado
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                // Popula dados de exemplo
                var u1 = new Usuario { IdUsuario = 1, Email = "tutor1@exemplo.com", Senha = "x", TipoUsuario = "TUTOR", DataCriacao = DateTime.UtcNow };
                var u2 = new Usuario { IdUsuario = 2, Email = "vet1@exemplo.com", Senha = "x", TipoUsuario = "VETERINARIO", DataCriacao = DateTime.UtcNow };
                db.Usuarios.AddRange(u1, u2);

                // Relacionados
                db.Tutores.Add(new Tutor { IdTutor = 1, NomeTutor = "Tutor Um", Cpf = "12345678901", IdUsuario = 1 });
                db.Veterinarios.Add(new Veterinario { IdVeterinario = 1, Nome = "Vet Um", Crmv = "CRMV123", IdUsuario = 2 });

                db.SaveChanges();
            });
        }
    }

    // Criando a Collection para compartilhar a Fixture
    [CollectionDefinition("ApiCollection")]
    public class ApiCollection : ICollectionFixture<ApiFactoryFixture>
    {
        // Esta classe não tem código. Serve apenas para aplicar a CollectionFixture.
    }
}
