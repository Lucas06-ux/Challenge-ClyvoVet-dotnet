using SuperVet.DTOs;

namespace SuperVet.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioResponseDto>> GetAllAsync();
        Task<UsuarioResponseDto?> GetByIdAsync(int id);
        Task<UsuarioResponseDto?> GetByEmailAsync(string email);
        Task<List<UsuarioResponseDto>> GetByTipoAsync(string tipo);
        Task<UsuarioResponseDto> CreateAsync(UsuarioRequestDto dto);
        Task<bool> UpdateAsync(int id, UsuarioRequestDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
