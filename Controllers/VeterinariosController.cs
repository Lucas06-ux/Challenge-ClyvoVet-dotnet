using Challenge_Sprints1e2.Data;
using Challenge_Sprints1e2.DTOs;
using Challenge_Sprints1e2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;  

namespace Challenge_Sprints1e2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VeterinariosController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        private static string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            var textoNormalizado = texto.Normalize(NormalizationForm.FormD);

            var caracteres = textoNormalizado
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray();

            return new string(caracteres)
                .Normalize(NormalizationForm.FormC)
                .ToUpper()
                .Trim();
        }

        public VeterinariosController(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        /// <summary>
        /// Carrega todos os veterinários
        /// </summary>
        /// <remarks>
        /// Retorna todos os veterinários cadastrados no sistema.
        ///
        /// Exemplo:
        ///
        ///     GET /api/veterinarios
        ///
        /// A resposta inclui nome, CRMV, telefone, especialidade,
        /// data de nascimento e usuário associado.
        /// </remarks>
        /// <returns>Lista de veterinários</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var veterinarios = await dbContext.Veterinarios.Select(v => new VeterinarioResponseDto
            {
                IdVeterinario = v.IdVeterinario, Nome = v.Nome, Crmv = v.Crmv, Telefone = v.Telefone,
                Especialidade = v.Especialidade, DataNascimento = v.DataNascimento, IdUsuario = v.IdUsuario, EmailUsuario = v.Usuario.Email
            }).ToListAsync();
           return Ok(veterinarios);
        }

        /// <summary>
        /// Busca veterinário pelo ID
        /// </summary>
        /// <param name="id">
        /// ID único do veterinário
        /// </param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     GET /api/veterinarios/1
        /// </remarks>
        /// <returns>
        /// Retorna um veterinário
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var veterinario = await dbContext.Veterinarios.Where(v => v.IdVeterinario == id).Select(v => new VeterinarioResponseDto
            {
                IdVeterinario = v.IdVeterinario, Nome = v.Nome, Crmv = v.Crmv, Telefone = v.Telefone, Especialidade = v.Especialidade,
                DataNascimento = v.DataNascimento, IdUsuario = v.IdUsuario, EmailUsuario = v.Usuario.Email
            }).FirstOrDefaultAsync();

            if (veterinario == null) return NotFound("Veterinário com o id " + id + " não encontrado.");

            return Ok(veterinario);
        }

        /// <summary>
        /// Busca veterinário pelo CRMV
        /// </summary>
        /// <param name="crmv">
        /// CRMV do veterinário
        /// </param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     GET /api/veterinarios/crmv/CRMVSP12345
        /// </remarks>
        /// <returns>
        /// Retorna um veterinário
        /// </returns>
        [HttpGet("crmv/{crmv}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCrmv(string crmv)
        {
            var veterinario = await dbContext.Veterinarios.Where(v => v.Crmv == crmv).Select(v => new VeterinarioResponseDto
            {
                IdVeterinario = v.IdVeterinario, Nome = v.Nome, Crmv = v.Crmv, Telefone = v.Telefone, Especialidade = v.Especialidade,
                DataNascimento = v.DataNascimento, IdUsuario = v.IdUsuario, EmailUsuario = v.Usuario.Email
            }).FirstOrDefaultAsync();
            if (veterinario == null) return NotFound("Veterinário com o CRMV " + crmv + " não encontrado.");
            return Ok(veterinario);
        }

        /// <summary>
        /// Busca veterinários por especialidade
        /// </summary>
        /// <param name="especialidade">
        /// Especialidade do veterinário
        /// </param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     GET /api/veterinarios/especialidade/Clinico
        /// </remarks>
        /// <returns>
        /// Lista de veterinários filtrados pela especialidade
        /// </returns>
        [HttpGet("especialidade/{especialidade}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByEspecialidade(string especialidade)
        {
            var especialidadeBusca = NormalizarTexto(especialidade);

            var veterinariosBanco = await dbContext.Veterinarios
                .Select(v => new VeterinarioResponseDto
                {
                    IdVeterinario = v.IdVeterinario, Nome = v.Nome, Crmv = v.Crmv,
                    Telefone = v.Telefone, Especialidade = v.Especialidade, DataNascimento = v.DataNascimento,
                    IdUsuario = v.IdUsuario, EmailUsuario = v.Usuario.Email
                })
                .ToListAsync();
            var veterinarios = veterinariosBanco
                .Where(v => NormalizarTexto(v.Especialidade).Contains(especialidadeBusca))
                .ToList();

            if (!veterinarios.Any())
            {
                return NotFound("Nenhum veterinário encontrado com a especialidade: " + especialidade);
            }

            return Ok(veterinarios);
        }

        /// <summary>
        /// Cadastra um novo veterinário
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /api/veterinarios
        ///
        ///     {
        ///         "nome":"Dra. Ana Souza",
        ///         "crmv":"CRMVSP12345",
        ///         "telefone":"11999999999",
        ///         "especialidade":"Clínico Geral",
        ///         "dataNascimento":"1990-03-20",
        ///         "emailUsuario":"vet@email.com"
        ///     }
        ///
        /// O ID é gerado automaticamente pelo banco.
        ///
        /// O usuário informado deve existir e possuir tipo VETERINARIO.
        /// </remarks>
        /// <param name="veterinarioToSave">
        /// Objeto do veterinário
        /// </param>
        /// <returns>
        /// Veterinário criado
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(VeterinarioRequestDto veterinarioToSave)
        {
            var telefoneLimpo = new string(veterinarioToSave.Telefone.Where(char.IsDigit).ToArray());

            if (telefoneLimpo.Length < 10 || telefoneLimpo.Length > 11)
                return BadRequest("Telefone inválido. Informe DDD + número. (ex: 11987654321)");

            veterinarioToSave.Telefone = telefoneLimpo;
            veterinarioToSave.Crmv = veterinarioToSave.Crmv.ToUpper().Replace(" ", "");

            if (!System.Text.RegularExpressions.Regex.IsMatch(
                veterinarioToSave.Crmv,
                @"^CRMV-[A-Z]{2}-\d{5}$"))
            {
                return BadRequest("CRMV inválido. Use o formato CRMV-UF-12345. Exemplo: CRMV-SP-12345.");
            }

            var usuario = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == veterinarioToSave.EmailUsuario && u.TipoUsuario == "VETERINARIO");

            if (usuario == null)
                return BadRequest("Usuário com e-mail " + veterinarioToSave.EmailUsuario + " não encontrado ou não é do tipo VETERINARIO.");

            if (await dbContext.Veterinarios.AnyAsync(v => v.Crmv == veterinarioToSave.Crmv))
                return BadRequest("CRMV " + veterinarioToSave.Crmv + " já cadastrado.");

            if (await dbContext.Veterinarios.AnyAsync(v => v.IdUsuario == usuario.IdUsuario))
                return BadRequest("Usuário já possui veterinário cadastrado.");

            var veterinario = new Veterinario
            {
                IdVeterinario = await dbContext.GetNextSequenceValueAsync("SEQ_VETERINARIO"),
                Nome = veterinarioToSave.Nome, Crmv = veterinarioToSave.Crmv, Telefone = veterinarioToSave.Telefone,
                Especialidade = veterinarioToSave.Especialidade, DataNascimento = veterinarioToSave.DataNascimento, IdUsuario = usuario.IdUsuario
            };

            dbContext.Veterinarios.Add(veterinario);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = veterinario.IdVeterinario }, new VeterinarioResponseDto
            {
                IdVeterinario = veterinario.IdVeterinario, Nome = veterinario.Nome, Crmv = veterinario.Crmv,
                Telefone = veterinario.Telefone, Especialidade = veterinario.Especialidade, DataNascimento = veterinario.DataNascimento,
                IdUsuario = veterinario.IdUsuario, EmailUsuario = usuario.Email
            });
        }

        /// <summary>
        /// Atualiza veterinário informando o ID
        /// </summary>
        /// <param name="id">
        /// ID do veterinário
        /// </param>
        /// <param name="veterinario">
        /// Dados atualizados
        /// </param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     PUT /api/veterinarios/1
        ///
        ///     {
        ///         "nome":"Dra. Ana Atualizada",
        ///         "crmv":"CRMVSP12345",
        ///         "telefone":"11988888888",
        ///         "especialidade":"Dermatologia",
        ///         "dataNascimento":"1990-03-20",
        ///         "emailUsuario":"vet@email.com"
        ///     }
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, VeterinarioRequestDto veterinario)
        {
            var veterinarioExistente = await dbContext.Veterinarios.FindAsync(id);

            if (veterinarioExistente == null)
                return NotFound("Veterinário com o id " + id + " não encontrado.");

            var telefoneLimpo = new string(veterinario.Telefone.Where(char.IsDigit).ToArray());

            if (telefoneLimpo.Length < 10 || telefoneLimpo.Length > 11)
                return BadRequest("Telefone inválido. Informe DDD + número. (ex: 11987654321)");

            veterinario.Telefone = telefoneLimpo;
            veterinario.Crmv = veterinario.Crmv.ToUpper().Replace(" ", "");

            if (!System.Text.RegularExpressions.Regex.IsMatch(
                veterinario.Crmv,
                @"^CRMV-[A-Z]{2}-\d{5}$"))
            {
                return BadRequest("CRMV inválido. Use o formato CRMV-UF-12345. Exemplo: CRMV-SP-12345.");
            }
            var usuario = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == veterinario.EmailUsuario && u.TipoUsuario == "VETERINARIO");

            if (usuario == null)
                return BadRequest("Usuário com e-mail " + veterinario.EmailUsuario + " não encontrado ou não é do tipo VETERINARIO.");

            if (await dbContext.Veterinarios.AnyAsync(v => v.Crmv == veterinario.Crmv && v.IdVeterinario != id))
                return BadRequest("CRMV " + veterinario.Crmv + " já cadastrado.");

            if (await dbContext.Veterinarios.AnyAsync(v => v.IdUsuario == usuario.IdUsuario && v.IdVeterinario != id))
                return BadRequest("Usuário já possui outro veterinário cadastrado.");

            veterinarioExistente.Nome = veterinario.Nome;
            veterinarioExistente.Crmv = veterinario.Crmv;
            veterinarioExistente.Telefone = veterinario.Telefone;
            veterinarioExistente.Especialidade = veterinario.Especialidade;
            veterinarioExistente.DataNascimento = veterinario.DataNascimento;
            veterinarioExistente.IdUsuario = usuario.IdUsuario;

            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Remove um veterinário informando o ID
        /// </summary>
        /// <param name="id">
        /// ID do veterinário
        /// </param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     DELETE /api/veterinarios/1
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var veterinario = await dbContext.Veterinarios.FindAsync(id);

            if (veterinario == null) return NotFound("Veterinário com o id " + id + " não encontrado.");

            dbContext.Veterinarios.Remove(veterinario);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
