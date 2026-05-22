using System.ComponentModel.DataAnnotations;

namespace Challenge_Sprints1e2.DTOs
{
    public class TutorRequestDto
    {
        [Required(ErrorMessage = "O nome do tutor é obrigatório.")]
        public string NomeTutor { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O email do usuário é obrigatório.")]
        public string EmailUsuario { get; set; } = string.Empty;
    }
}
