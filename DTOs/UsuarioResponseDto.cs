namespace Challenge_Sprints1e2.DTOs
{
    public class UsuarioResponseDto
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
    }
}
