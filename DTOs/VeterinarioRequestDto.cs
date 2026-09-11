using System.ComponentModel.DataAnnotations;

namespace SuperVet.DTOs
{
    public class VeterinarioRequestDto
    {
        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string Crmv { get; set; } = string.Empty;

        [Required]
        public string Telefone { get; set; } = string.Empty;

        [Required]
        public string Especialidade { get; set; } = string.Empty;

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public string EmailUsuario { get; set; } = string.Empty;
    }
}
