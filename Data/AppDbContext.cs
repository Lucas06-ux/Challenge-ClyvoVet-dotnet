using SuperVet.Models;
using Microsoft.EntityFrameworkCore;

namespace SuperVet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tutor> Tutores { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
     

        public async Task<int> GetNextSequenceValueAsync(string sequenceName)
        {
            // Se o provedor for relacional (ex.: Oracle) usa a sequence
            if (Database.IsRelational())
            {
                var connection = Database.GetDbConnection();

                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                await using var command = connection.CreateCommand();
                command.CommandText = $"SELECT {sequenceName}.NEXTVAL FROM DUAL";

                var result = await command.ExecuteScalarAsync();

                return Convert.ToInt32(result);
            }

            // Fallback para provedores não-relacionais (ex.: InMemory) utilizado nos testes.
            // Retorna próximo Id baseado no maior Id já presente em cada DbSet.
            sequenceName = sequenceName?.ToUpperInvariant() ?? string.Empty;
            return sequenceName switch
            {
                "SEQ_USUARIO" => (await Usuarios.AnyAsync()) ? await Usuarios.MaxAsync(u => u.IdUsuario) + 1 : 1,
                "SEQ_TUTOR" => (await Tutores.AnyAsync()) ? await Tutores.MaxAsync(t => t.IdTutor) + 1 : 1,
                "SEQ_VETERINARIO" => (await Veterinarios.AnyAsync()) ? await Veterinarios.MaxAsync(v => v.IdVeterinario) + 1 : 1,
                _ => 1
            };
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


              modelBuilder.Entity<Usuario>()
                .HasCheckConstraint("CK_USUARIO_TIPO", "TIPO_USER IN ('TUTOR','VETERINARIO')");

            modelBuilder.Entity<Usuario>()
               .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Tutor>()
                .HasIndex(t => t.Cpf)
               .IsUnique();

            modelBuilder.Entity<Tutor>()
                .HasIndex(t => t.IdUsuario)
                .IsUnique();

            modelBuilder.Entity<Veterinario>()
               .HasIndex(v => v.Crmv)
               .IsUnique();

            modelBuilder.Entity<Veterinario>()
                .HasIndex(v => v.IdUsuario)
                .IsUnique();

        }

    }
}
