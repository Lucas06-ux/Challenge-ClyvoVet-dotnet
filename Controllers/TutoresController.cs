using Challenge_Sprints1e2.Data;
using Challenge_Sprints1e2.DTOs;
using Challenge_Sprints1e2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Challenge_Sprints1e2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TutoresController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public TutoresController(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        /// <summary>
        /// Carrega todos os tutores
        /// </summary>
        /// <remarks>
        /// Retorna todos os tutores cadastrados no sistema.
        ///
        /// Exemplo:
        ///
        ///     GET /api/tutores
        ///
        /// A resposta inclui nome, CPF, telefone,
        /// data de nascimento e usuário associado.
        /// </remarks>
        /// <returns>Lista de tutores</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var tutores = await dbContext.Tutores.Select(t => new TutorResponseDto
            {
                IdTutor = t.IdTutor, NomeTutor = t.NomeTutor, Cpf = t.Cpf, Telefone = t.Telefone, DataNascimento = t.DataNascimento,
                IdUsuario = t.IdUsuario, EmailUsuario = t.Usuario.Email
            }).ToListAsync();
            return Ok(tutores);
        }

        /// <summary>
        /// Busca tutor pelo ID
        /// </summary>
        /// <param name="id">
        /// ID único do tutor
        /// </param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     GET /api/tutores/1
        /// </remarks>
        /// <returns>
        /// Retorna um tutor
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var tutor = await dbContext.Tutores.Where(t => t.IdTutor == id).Select(t => new TutorResponseDto
            {
                IdTutor = t.IdTutor,  NomeTutor = t.NomeTutor, Cpf = t.Cpf, Telefone = t.Telefone, DataNascimento = t.DataNascimento,
                IdUsuario = t.IdUsuario, EmailUsuario = t.Usuario.Email
            }).FirstOrDefaultAsync();
            if (tutor == null) return NotFound("Tutor com o id " + id + " não encontrado.");
            return Ok(tutor);
        }

        /// <summary>
        /// Busca tutor pelo CPF
        /// </summary>
        /// <param name="cpf">
        /// CPF do tutor
        /// </param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     GET /api/tutores/cpf/12345678900
        /// </remarks>
        /// <returns>
        /// Retorna um tutor
        /// </returns>
        [HttpGet("cpf/{cpf}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            var tutor = await dbContext.Tutores.Where(t => t.Cpf == cpf).Select(t => new TutorResponseDto
            {
                IdTutor = t.IdTutor, NomeTutor = t.NomeTutor, Cpf = t.Cpf, Telefone = t.Telefone, DataNascimento = t.DataNascimento,
                IdUsuario = t.IdUsuario, EmailUsuario = t.Usuario.Email
            }).FirstOrDefaultAsync();
            if (tutor == null) return NotFound("Tutor com o CPF " + cpf + " não encontrado.");
            return Ok(tutor);
        }

        /// <summary>
        /// Cadastra um novo tutor
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /api/tutores
        ///
        ///     {
        ///         "nomeTutor":"Lucas Giannini",
        ///         "cpf":"12345678900",
        ///         "telefone":"11999999999",
        ///         "dataNascimento":"2006-05-30",
        ///         "emailUsuario":"lucas@email.com"
        ///     }
        ///
        /// O ID é gerado automaticamente pelo banco.
        ///
        /// O usuário informado deve existir e possuir
        /// tipo TUTOR.
        /// </remarks>
        /// <param name="tutorToSave">
        /// Objeto do tutor
        /// </param>
        /// <returns>
        /// Tutor criado
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(TutorRequestDto tutorToSave)
        {
            var cpfLimpo = new string(tutorToSave.Cpf.Where(char.IsDigit).ToArray());

            if (cpfLimpo.Length != 11)
                return BadRequest("CPF inválido. Informe exatamente 11 números.");

            tutorToSave.Cpf = cpfLimpo;
            var telefoneLimpo = new string(tutorToSave.Telefone.Where(char.IsDigit).ToArray());

            if (telefoneLimpo.Length < 10 || telefoneLimpo.Length > 11)
                return BadRequest("Telefone inválido. Informe DDD + número.(ex: 11987654321)");
            tutorToSave.Telefone = telefoneLimpo;

            var usuario = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == tutorToSave.EmailUsuario && u.TipoUsuario == "TUTOR");

            if (usuario == null)
                return BadRequest("Usuário com e-mail " + tutorToSave.EmailUsuario + " não encontrado ou não é do tipo TUTOR.");

            if (await dbContext.Tutores.AnyAsync(t => t.Cpf == tutorToSave.Cpf))
                return BadRequest("CPF " + tutorToSave.Cpf + " já cadastrado.");

            if (await dbContext.Tutores.AnyAsync(t => t.IdUsuario == usuario.IdUsuario))
                return BadRequest("Usuário já possui tutor cadastrado.");

            var tutor = new Tutor
            {
                IdTutor = await dbContext.GetNextSequenceValueAsync("SEQ_TUTOR"),
                NomeTutor = tutorToSave.NomeTutor, Cpf = tutorToSave.Cpf, Telefone = tutorToSave.Telefone,
                DataNascimento = tutorToSave.DataNascimento, IdUsuario = usuario.IdUsuario
            };

            dbContext.Tutores.Add(tutor);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = tutor.IdTutor }, new TutorResponseDto
            {
                IdTutor = tutor.IdTutor, NomeTutor = tutor.NomeTutor, Cpf = tutor.Cpf, Telefone = tutor.Telefone,
                DataNascimento = tutor.DataNascimento, IdUsuario = tutor.IdUsuario, EmailUsuario = usuario.Email
            });
        }

        /// <summary>
        /// Atualiza tutor informando o ID
        /// </summary>
        /// <param name="id">
        /// ID do tutor
        /// </param>
        /// <param name="tutor">
        /// Dados atualizados
        /// </param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     PUT /api/tutores/1
        ///
        ///     {
        ///         "nomeTutor":"Lucas Atualizado",
        ///         "cpf":"12345678900",
        ///         "telefone":"11888888888",
        ///         "dataNascimento":"2006-05-30",
        ///         "emailUsuario":"lucas@email.com"
        ///     }
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, TutorRequestDto tutor)
        {
            var tutorExistente = await dbContext.Tutores.FindAsync(id);

            if (tutorExistente == null)
                return NotFound("Tutor com o id " + id + " não encontrado.");

            var cpfLimpo = new string(tutor.Cpf.Where(char.IsDigit).ToArray());

            if (cpfLimpo.Length != 11)
                return BadRequest("CPF inválido. Informe exatamente 11 números.");

            tutor.Cpf = cpfLimpo;

            var telefoneLimpo = new string(tutor.Telefone.Where(char.IsDigit).ToArray());

            if (telefoneLimpo.Length < 10 || telefoneLimpo.Length > 11)
                return BadRequest("Telefone inválido. Informe DDD + número. (ex: 11987654321)");

            tutor.Telefone = telefoneLimpo;
            var usuario = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == tutor.EmailUsuario && u.TipoUsuario == "TUTOR");

            if (usuario == null)
                return BadRequest("Usuário com e-mail " + tutor.EmailUsuario + " não encontrado ou não é do tipo TUTOR.");

            if (await dbContext.Tutores.AnyAsync(t => t.Cpf == tutor.Cpf && t.IdTutor != id))
                return BadRequest("CPF " + tutor.Cpf + " já cadastrado.");

            if (await dbContext.Tutores.AnyAsync(t => t.IdUsuario == usuario.IdUsuario && t.IdTutor != id))
                return BadRequest("Usuário já possui outro tutor cadastrado.");

            tutorExistente.NomeTutor = tutor.NomeTutor;
            tutorExistente.Cpf = tutor.Cpf;
            tutorExistente.Telefone = tutor.Telefone;
            tutorExistente.DataNascimento = tutor.DataNascimento;
            tutorExistente.IdUsuario = usuario.IdUsuario;
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Remove um tutor informando o ID
        /// </summary>
        /// <param name="id">
        /// ID do tutor
        /// </param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     DELETE /api/tutores/1
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var tutor = await dbContext.Tutores.FindAsync(id);
            if (tutor == null) return NotFound("Tutor com o id " + id + " não encontrado.");
            dbContext.Tutores.Remove(tutor);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
