using Challenge_Sprints1e2.Models;
using Microsoft.EntityFrameworkCore;

namespace Challenge_Sprints1e2.Data
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
