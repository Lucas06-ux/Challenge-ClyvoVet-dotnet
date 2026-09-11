using SuperVet.Data;
using SuperVet.Services;
using SuperVet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperVet.DTOs;

namespace SuperVet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;
        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Carrega todos os usuários
        /// </summary>
        /// <remarks>
        /// Retorna todos os usuários cadastrados no sistema.
        ///
        /// Exemplo:
        ///
        ///     GET /api/usuarios
        ///
        /// A resposta inclui e-mail, tipo de usuário e data de criação.
        /// </remarks>
        /// <returns>Lista de usuários</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _service.GetAllAsync();
            return Ok(usuarios);
        }

        /// <summary>
        /// Busca usuário pelo ID
        /// </summary>
        /// <param name="id">ID único do usuário</param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     GET /api/usuarios/1
        /// </remarks>
        /// <returns>Retorna um usuário</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null) return NotFound($"Usuário de ID {id} não encontrado.");
            return Ok(usuario);
        }

        /// <summary>
        /// Busca usuário pelo e-mail 
        /// </summary>
        /// <param name="email">E-mail do usuário</param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     GET /api/usuarios/email/lucas@email.com
        /// </remarks>
        /// <returns>Retorna um usuário</returns>
        [HttpGet("email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var usuario = await _service.GetByEmailAsync(email);
            if (usuario == null) return NotFound($"Usuário com o e-mail {email} não encontrado.");
            return Ok(usuario);
        }

        /// <summary>
        /// Busca todos os usuários pelo tipo informado 
        /// </summary>
        /// <param name="tipo">
        /// Tipo do usuário (TUTOR ou VETERINARIO)
        /// </param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     GET /api/usuarios/tipo/TUTOR
        ///
        /// Retorna todos os usuários do tipo informado.
        /// </remarks>
        /// <returns>
        /// Lista de usuários filtrados pelo tipo
        /// </returns>
        [HttpGet("tipo/{tipo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByTipo(string tipo)
        {
            tipo = tipo.ToUpper();

            if (tipo != "TUTOR" && tipo != "VETERINARIO")
            {
                return BadRequest("Usuário com o tipo " + tipo + " inválido. Informe TUTOR ou VETERINARIO.");
            }
            var usuarios = await _service.GetByTipoAsync(tipo);
            return Ok(usuarios);
        }

        /// <summary>
        /// Cadastra um novo usuário no sistema
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /api/usuarios
        ///
        ///     {
        ///         "email": "lucas@email.com",
        ///         "senha": "123456",
        ///         "tipoUsuario": "TUTOR"
        ///     }
        ///
        /// O ID é gerado automaticamente pelo banco.
        /// </remarks>
        /// <param name="usuarioToSave">Objeto do usuário</param>
        /// <returns>Usuário criado</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(UsuarioRequestDto usuarioToSave)
        {
            try
            {
                var created = await _service.CreateAsync(usuarioToSave);
                return CreatedAtAction(nameof(GetById), new { id = created.IdUsuario }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza usuário informando o ID
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="usuario">Dados atualizados</param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     PUT /api/usuarios/1
        ///
        ///     {
        ///         "email": "novo@email.com",
        ///         "senha": "novaSenha",
        ///         "tipoUsuario": "VETERINARIO"
        ///     }
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id,UsuarioRequestDto usuario
        )
        {
            try
            {
                var updated = await _service.UpdateAsync(id, usuario);
                return updated ? NoContent() : NotFound($"Usuário com o id {id} não encontrado.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Remove um usuário informando o ID
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <remarks>
        /// Exemplo:
        ///
        ///     DELETE /api/usuarios/1
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                return deleted ? NoContent() : NotFound($"Usuário com o id {id} não encontrado no sistema.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

