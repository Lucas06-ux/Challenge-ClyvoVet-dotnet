using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenge_Sprints1e2.Models
{
    //TABELA DO BANCO DE DADOS 
    [Table("TB_VETERINARIO")]
    [Index(nameof(Crmv), IsUnique = true)]//CRMV DEFINIDO COMO ÚNICO PARA EVITAR DUPLICIDADE
    [Index(nameof(IdUsuario), IsUnique = true)]
    public class Veterinario
    {
        [Key]
        [Column("ID_VETERINARIO")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdVeterinario { get; set; }

        [Required]
        [Column("NOME")]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [Column("CRMV")]
        [StringLength(14)]

        [RegularExpression(@"^CRMV-[A-Z]{2}-\d{5}$",ErrorMessage ="Formato inválido. Use CRMV-UF-12345")]
        public string Crmv { get; set; } = string.Empty;

        [Required]
        [Column("TELEFONE")]
        [MaxLength(15)]
        public string Telefone { get; set; } = string.Empty;

        [Required]
        [Column("ESPECIALIDADE")]
        [MaxLength(100)]
        public string Especialidade { get; set; } = string.Empty;

        [Required]
        [Column("DT_NASCIMENTO")]
        public DateTime DataNascimento { get; set; }

        [Required]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; } = null!;
    }
}
