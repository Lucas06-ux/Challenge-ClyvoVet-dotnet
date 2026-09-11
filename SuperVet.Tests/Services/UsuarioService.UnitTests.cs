using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperVet.Data;
using SuperVet.DTOs;
using SuperVet.Models;
using SuperVet.Services;
using Xunit;

namespace SuperVet.Tests.Services
{
    public class UsuarioService_UnitTests
    {
        private AppDbContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_QuandoDadosValidos_RetornaUsuarioCriado()
        {
            // Arrange
            using var db = CreateContext(nameof(CreateAsync_QuandoDadosValidos_RetornaUsuarioCriado));
            var service = new UsuarioService(db);

            var dto = new UsuarioRequestDto { Email = "novo@unit.com", Senha = "x", TipoUsuario = "TUTOR" };

            // Act
            var created = await service.CreateAsync(dto);

            // Assert
            Assert.NotNull(created);
            Assert.Equal(dto.Email, created.Email);
            Assert.Equal("TUTOR", created.TipoUsuario);
            Assert.True(created.IdUsuario > 0);
        }

        [Fact]
        public async Task CreateAsync_QuandoEmailDuplicado_LancaInvalidOperationException()
        {
            // Arrange
            using var db = CreateContext(nameof(CreateAsync_QuandoEmailDuplicado_LancaInvalidOperationException));
            db.Usuarios.Add(new Usuario { IdUsuario = 1, Email = "dup@dup.com", Senha = "x", TipoUsuario = "TUTOR", DataCriacao = DateTime.UtcNow });
            await db.SaveChangesAsync();

            var service = new UsuarioService(db);
            var dto = new UsuarioRequestDto { Email = "dup@dup.com", Senha = "x", TipoUsuario = "TUTOR" };

            // Act + Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(dto));
            Assert.Contains("E-mail já cadastrado", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_QuandoTipoInvalido_LancaInvalidOperationException()
        {
            // Arrange
            using var db = CreateContext(nameof(UpdateAsync_QuandoTipoInvalido_LancaInvalidOperationException));
            db.Usuarios.Add(new Usuario { IdUsuario = 1, Email = "a@a.com", Senha = "x", TipoUsuario = "TUTOR", DataCriacao = DateTime.UtcNow });
            await db.SaveChangesAsync();

            var service = new UsuarioService(db);
            var dto = new UsuarioRequestDto { Email = "a@a.com", Senha = "x", TipoUsuario = "INVALID" };

            // Act + Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(1, dto));
            Assert.Contains("Tipo de usuário inválido", ex.Message);
        }

        [Fact]
        public async Task UpdateAsync_QuandoEmailJaExiste_LancaInvalidOperationException()
        {
            // Arrange
            using var db = CreateContext(nameof(UpdateAsync_QuandoEmailJaExiste_LancaInvalidOperationException));
            db.Usuarios.Add(new Usuario { IdUsuario = 1, Email = "one@ex.com", Senha = "x", TipoUsuario = "TUTOR", DataCriacao = DateTime.UtcNow });
            db.Usuarios.Add(new Usuario { IdUsuario = 2, Email = "two@ex.com", Senha = "x", TipoUsuario = "VETERINARIO", DataCriacao = DateTime.UtcNow });
            await db.SaveChangesAsync();

            var service = new UsuarioService(db);
            var dto = new UsuarioRequestDto { Email = "two@ex.com", Senha = "x", TipoUsuario = "TUTOR" };

            // Act + Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateAsync(1, dto));
            Assert.Contains("E-mail já cadastrado", ex.Message);
        }

        [Fact]
        public async Task DeleteAsync_QuandoPossuiTutor_LancaInvalidOperationException()
        {
            // Arrange
            using var db = CreateContext(nameof(DeleteAsync_QuandoPossuiTutor_LancaInvalidOperationException));
            db.Usuarios.Add(new Usuario { IdUsuario = 1, Email = "t@t.com", Senha = "x", TipoUsuario = "TUTOR", DataCriacao = DateTime.UtcNow });
            db.Tutores.Add(new Tutor { IdTutor = 1, NomeTutor = "T", Cpf = "123", IdUsuario = 1 });
            await db.SaveChangesAsync();

            var service = new UsuarioService(db);

            // Act + Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(1));
            Assert.Contains("Apague a conta de tutor", ex.Message);
        }

        [Fact]
        public async Task DeleteAsync_QuandoNaoPossuiRelacionados_RemoveERetornaTrue()
        {
            // Arrange
            using var db = CreateContext(nameof(DeleteAsync_QuandoNaoPossuiRelacionados_RemoveERetornaTrue));
            db.Usuarios.Add(new Usuario { IdUsuario = 1, Email = "t2@t.com", Senha = "x", TipoUsuario = "TUTOR", DataCriacao = DateTime.UtcNow });
            await db.SaveChangesAsync();

            var service = new UsuarioService(db);

            // Act
            var result = await service.DeleteAsync(1);

            // Assert
            Assert.True(result);
            var exists = await db.Usuarios.FindAsync(1);
            Assert.Null(exists);
        }
    }
}
