namespace Challenge_Sprints1e2.DTOs
{
    public class TutorResponseDto
    {
        public int IdTutor { get; set; }
        public string NomeTutor { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public int IdUsuario { get; set; }
        public string EmailUsuario { get; set; } = string.Empty;
    }
}
