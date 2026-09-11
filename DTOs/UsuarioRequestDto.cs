using System.ComponentModel.DataAnnotations;

namespace SuperVet.DTOs
{
    public class UsuarioRequestDto
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
        public string TipoUsuario { get; set; } = string.Empty;
    }
}
