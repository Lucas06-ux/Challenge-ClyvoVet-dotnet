using Challenge_Sprints1e2.Data;
using Challenge_Sprints1e2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Challenge_Sprints1e2.DTOs;

namespace Challenge_Sprints1e2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        public UsuariosController(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
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
            var usuarios = await dbContext.Usuarios.Select(u => new UsuarioResponseDto
       {
           IdUsuario = u.IdUsuario, Email = u.Email, TipoUsuario = u.TipoUsuario, DataCriacao = u.DataCriacao
       })
       .ToListAsync();
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
            var usuario = await dbContext.Usuarios.Where(u => u.IdUsuario == id).Select(u => new UsuarioResponseDto
       {
           IdUsuario = u.IdUsuario, Email = u.Email, TipoUsuario = u.TipoUsuario, DataCriacao = u.DataCriacao
       })
            .FirstOrDefaultAsync();
            if (usuario == null)
            {
                return NotFound("Usuário de ID " + id + " não encontrado.");
            }
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
            var usuario = await dbContext.Usuarios.Where(u => u.Email == email).Select(u => new UsuarioResponseDto
        {
            IdUsuario = u.IdUsuario, Email = u.Email, TipoUsuario = u.TipoUsuario, DataCriacao = u.DataCriacao
        })
        .FirstOrDefaultAsync();
            if (usuario == null)
            {
                return NotFound("Usuário com o e-mail " + email + " não encontrado.");
            }
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
            var usuarios =
                await dbContext.Usuarios.Where(u =>u.TipoUsuario == tipo)
                .Select(u =>new UsuarioResponseDto
                    {IdUsuario =  u.IdUsuario, Email = u.Email, TipoUsuario = u.TipoUsuario, DataCriacao = u.DataCriacao
                    }) .ToListAsync();
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
            usuarioToSave.TipoUsuario = usuarioToSave.TipoUsuario.ToUpper();
            if (usuarioToSave.TipoUsuario != "TUTOR" && usuarioToSave.TipoUsuario != "VETERINARIO")
            {
                return BadRequest("Tipo de usuário " + usuarioToSave.TipoUsuario + " inválido (Tutor ou Veterinário).");
            }
            var emailJaExiste = await dbContext.Usuarios.AnyAsync(u => u.Email == usuarioToSave.Email);
            if (emailJaExiste)
            {
                return BadRequest("E-mail " + usuarioToSave.Email + " já cadastrado.");
            }
            var usuario = new Usuario
            { IdUsuario = await dbContext .GetNextSequenceValueAsync("SEQ_USUARIO"), Email = usuarioToSave.Email, Senha = usuarioToSave.Senha,
                TipoUsuario = usuarioToSave.TipoUsuario, DataCriacao = DateTime.Now 
            };
            dbContext.Usuarios.Add(usuario);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction( nameof(GetById), new { id = usuario.IdUsuario },new UsuarioResponseDto 
                {
                    IdUsuario = usuario.IdUsuario, Email = usuario.Email, TipoUsuario = usuario.TipoUsuario, DataCriacao = usuario.DataCriacao
                });
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
            var usuarioExistente =
                await dbContext.Usuarios.FindAsync(id);
            if (usuarioExistente == null)
            {
                return NotFound("Usuário com o id " + id + " não encontrado.");
            }
            usuario.TipoUsuario = usuario.TipoUsuario.ToUpper();
            if (usuario.TipoUsuario != "TUTOR" && usuario.TipoUsuario != "VETERINARIO")
            {
                return BadRequest("Tipo de usuário " + usuario.TipoUsuario + " inválido. Informe apenas TUTOR ou VETERINARIO.");
            }
            var emailJaExiste = await dbContext.Usuarios
                .AnyAsync(u => u.Email == usuario.Email && u.IdUsuario != id);
            if (emailJaExiste)
            {
                return BadRequest( "E-mail " + usuario.Email + " já cadastrado no sistema. Informe outro e-mail.");
            }
            usuarioExistente.Email = usuario.Email;
            usuarioExistente.Senha = usuario.Senha;
            usuarioExistente.TipoUsuario = usuario.TipoUsuario;
            await dbContext.SaveChangesAsync();
            return NoContent();
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
            var usuario = await dbContext.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound("Usuário com o id " + id + " não encontrado no sistema.");
            }

            var possuiTutor = await dbContext.Tutores.AnyAsync(t => t.IdUsuario == id);

            if (possuiTutor)
            {
                return BadRequest("Impossível remover este usuário. Apague a conta de tutor primeiro.");
            }

            var possuiVeterinario = await dbContext.Veterinarios.AnyAsync(v => v.IdUsuario == id);

            if (possuiVeterinario)
            {
                return BadRequest("Impossível remover este usuário. Apague a conta de veterinário primeiro.");
            }

            dbContext.Usuarios.Remove(usuario);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}

