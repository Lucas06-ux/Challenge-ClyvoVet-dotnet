using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SuperVet.Controllers;
using SuperVet.DTOs;
using SuperVet.Services;
using Xunit;

namespace SuperVet.Tests.Controllers
{
    public class UsuariosController_UnitTests
    {
        private readonly Mock<IUsuarioService> _mockService;
        private readonly UsuariosController _controller;

        public UsuariosController_UnitTests()
        {
            _mockService = new Mock<IUsuarioService>();
            _controller = new UsuariosController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_QuandoExistemUsuarios_RetornaOkComLista()
        {
            // Arrange
            var expected = new List<UsuarioResponseDto>
            {
                new UsuarioResponseDto { IdUsuario = 1, Email = "Ellie@email.com", TipoUsuario = "TUTOR" },
                new UsuarioResponseDto { IdUsuario = 2, Email = "Joel@email.com", TipoUsuario = "VETERINARIO" }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(expected);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var list = Assert.IsType<List<UsuarioResponseDto>>(ok.Value);
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task GetById_QuandoUsuarioExiste_RetornaOkComUsuario()
        {
            // Arrange
            var dto = new UsuarioResponseDto { IdUsuario = 1, Email = "x@y.com", TipoUsuario = "TUTOR" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var ret = Assert.IsType<UsuarioResponseDto>(ok.Value);
            Assert.Equal(dto.Email, ret.Email);
        }

        [Fact]
        public async Task GetById_QuandoUsuarioNaoExiste_RetornaNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((UsuarioResponseDto?)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Create_QuandoDadosValidos_RetornaCreated()
        {
            // Arrange
            var request = new UsuarioRequestDto { Email = "novo@novo.com", Senha = "s", TipoUsuario = "TUTOR" };
            var created = new UsuarioResponseDto { IdUsuario = 10, Email = request.Email, TipoUsuario = request.TipoUsuario };
            _mockService.Setup(s => s.CreateAsync(request)).ReturnsAsync(created);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdAt = Assert.IsType<CreatedAtActionResult>(result);
            var ret = Assert.IsType<UsuarioResponseDto>(createdAt.Value);
            Assert.Equal(10, ret.IdUsuario);
        }

        [Fact]
        public async Task Create_QuandoEmailDuplicado_RetornaBadRequest()
        {
            // Arrange
            var request = new UsuarioRequestDto { Email = "dup@dup.com", Senha = "s", TipoUsuario = "TUTOR" };
            _mockService.Setup(s => s.CreateAsync(request)).ThrowsAsync(new InvalidOperationException("E-mail já cadastrado"));

            // Act
            var result = await _controller.Create(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_QuandoExiste_RetornaNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_QuandoNaoExiste_RetornaNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
