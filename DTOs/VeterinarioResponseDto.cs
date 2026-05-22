namespace Challenge_Sprints1e2.DTOs
{
    public class VeterinarioResponseDto
    {
        public int IdVeterinario { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Crmv { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public int IdUsuario { get; set; }
        public string EmailUsuario { get; set; } = string.Empty;
    }
}
