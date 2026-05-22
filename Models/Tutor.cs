using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenge_Sprints1e2.Models
{
    //TABELA DO BANCO DE DADOS 
    [Table("TB_TUTOR")]
    [Index(nameof(Cpf), IsUnique = true)]//CPF DEFINIDO COMO ÚNICO PARA EVITAR DUPLICIDADE
    [Index(nameof(IdUsuario), IsUnique = true)]
    public class Tutor
    {
        [Key]
        [Column("ID_TUTOR")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdTutor { get; set; }

        [Required]
        [Column("NOME_TUTOR")]
        [MaxLength(100)]
        public string NomeTutor { get; set; } = string.Empty;

        [Required]
        [Column("CPF")]
        [MaxLength(14)]
        public string Cpf { get; set; } = string.Empty;

        [Required]
        [Column("TELEFONE")]
        [MaxLength(15)]
        public string Telefone { get; set; } = string.Empty;

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
