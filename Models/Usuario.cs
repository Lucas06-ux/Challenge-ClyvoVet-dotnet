using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenge_Sprints1e2.Models
{

        //TABELA DO BANCO DE DADOS 
        [Table("TB_USUARIO")]
        [Index(nameof(Email), IsUnique = true)]//EMAIL DEFINIDO COMO ÚNICO PARA EVITAR DUPLICIDADE
    public class Usuario
        {
            [Key]
            [Column("ID_USUARIO")]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            public int IdUsuario { get; set; }

            [Required]
            [Column("EMAIL")]
            [MaxLength(80)]
            public string Email { get; set; }

            [Required]
            [Column("SENHA")]
            [MaxLength(100)]
            public string Senha { get; set; }

            [Required]
            [Column("TIPO_USER")]
            [MaxLength(35)]
            public string TipoUsuario { get; set; }

            [Required]
            [Column("DATA_CRIACAO")]
            public DateTime DataCriacao { get; set; }

            // RELACIONAMENTOS

            public Tutor? Tutor { get; set; }

           // public Veterinario? Veterinario { get; set; }
    }
    
}
