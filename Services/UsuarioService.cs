using Microsoft.EntityFrameworkCore;
using SuperVet.Data;
using SuperVet.DTOs;
using SuperVet.Models;

namespace SuperVet.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _db;
        public UsuarioService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<UsuarioResponseDto>> GetAllAsync()
        {
            return await _db.Usuarios
                .Select(u => new UsuarioResponseDto { IdUsuario = u.IdUsuario, Email = u.Email, TipoUsuario = u.TipoUsuario, DataCriacao = u.DataCriacao })
                .ToListAsync();
        }

        public async Task<UsuarioResponseDto?> GetByIdAsync(int id)
        {
            return await _db.Usuarios.Where(u => u.IdUsuario == id)
                .Select(u => new UsuarioResponseDto { IdUsuario = u.IdUsuario, Email = u.Email, TipoUsuario = u.TipoUsuario, DataCriacao = u.DataCriacao })
                .FirstOrDefaultAsync();
        }

        public async Task<UsuarioResponseDto?> GetByEmailAsync(string email)
        {
            return await _db.Usuarios.Where(u => u.Email == email)
                .Select(u => new UsuarioResponseDto { IdUsuario = u.IdUsuario, Email = u.Email, TipoUsuario = u.TipoUsuario, DataCriacao = u.DataCriacao })
                .FirstOrDefaultAsync();
        }

        public async Task<List<UsuarioResponseDto>> GetByTipoAsync(string tipo)
        {
            return await _db.Usuarios.Where(u => u.TipoUsuario == tipo)
                .Select(u => new UsuarioResponseDto { IdUsuario = u.IdUsuario, Email = u.Email, TipoUsuario = u.TipoUsuario, DataCriacao = u.DataCriacao })
                .ToListAsync();
        }

        public async Task<UsuarioResponseDto> CreateAsync(UsuarioRequestDto dto)
        {
            dto.TipoUsuario = dto.TipoUsuario.ToUpper();
            var emailJaExiste = await _db.Usuarios.AnyAsync(u => u.Email == dto.Email);
            if (emailJaExiste)
            {
                throw new InvalidOperationException("E-mail já cadastrado");
            }

            var usuario = new Usuario
            {
                IdUsuario = await _db.GetNextSequenceValueAsync("SEQ_USUARIO"),
                Email = dto.Email,
                Senha = dto.Senha,
                TipoUsuario = dto.TipoUsuario,
                DataCriacao = DateTime.Now
            };

            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            return new UsuarioResponseDto { IdUsuario = usuario.IdUsuario, Email = usuario.Email, TipoUsuario = usuario.TipoUsuario, DataCriacao = usuario.DataCriacao };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (usuario == null) return false;

            // Impede remoção se existirem entidades relacionadas
            var possuiTutor = await _db.Tutores.AnyAsync(t => t.IdUsuario == id);
            if (possuiTutor) throw new InvalidOperationException("Impossível remover este usuário. Apague a conta de tutor primeiro.");

            var possuiVeterinario = await _db.Veterinarios.AnyAsync(v => v.IdUsuario == id);
            if (possuiVeterinario) throw new InvalidOperationException("Impossível remover este usuário. Apague a conta de veterinário primeiro.");

            _db.Usuarios.Remove(usuario);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(int id, UsuarioRequestDto dto)
        {
            var usuarioExistente = await _db.Usuarios.FindAsync(id);
            if (usuarioExistente == null) return false;

            dto.TipoUsuario = dto.TipoUsuario.ToUpper();
            if (dto.TipoUsuario != "TUTOR" && dto.TipoUsuario != "VETERINARIO")
            {
                throw new InvalidOperationException("Tipo de usuário inválido");
            }

            var emailJaExiste = await _db.Usuarios.AnyAsync(u => u.Email == dto.Email && u.IdUsuario != id);
            if (emailJaExiste) throw new InvalidOperationException("E-mail já cadastrado");

            usuarioExistente.Email = dto.Email;
            usuarioExistente.Senha = dto.Senha;
            usuarioExistente.TipoUsuario = dto.TipoUsuario;

            _db.Usuarios.Update(usuarioExistente);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
