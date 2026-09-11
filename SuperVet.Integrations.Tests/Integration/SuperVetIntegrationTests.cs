using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net;
using Xunit;
using SuperVet.Integrations.Tests.FactoryFixture;
using SuperVet.DTOs;

namespace SuperVet.Integrations.Tests.Integration
{
    [Collection("ApiCollection")]
    public class SuperVetIntegrationTests
    {
        private readonly ApiFactoryFixture _factory;
        private readonly System.Net.Http.HttpClient _client;

        public SuperVetIntegrationTests(ApiFactoryFixture factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Health_RetornaOk()
        {
            var response = await _client.GetAsync("/health");
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetTutores_RetornaOkELista()
        {
            var response = await _client.GetAsync("/api/Tutores");
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var lista = await response.Content.ReadFromJsonAsync<List<object>>();
            Assert.NotNull(lista);
        }

        [Fact]
        public async Task PostUsuario_CriaComSucesso()
        {
            var dto = new UsuarioRequestDto { Email = "novo@exemplo.com", Senha = "123", TipoUsuario = "TUTOR" };
            var response = await _client.PostAsJsonAsync("/api/Usuarios", dto);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var created = await response.Content.ReadFromJsonAsync<UsuarioResponseDto>();
            Assert.NotNull(created);
            Assert.Equal(dto.Email, created.Email);
        }

        [Fact]
        public async Task PostUsuario_EmailDuplicado_RetornaBadRequest()
        {
            var dto = new UsuarioRequestDto { Email = "duplicado@exemplo.com", Senha = "x", TipoUsuario = "TUTOR" };
            var first = await _client.PostAsJsonAsync("/api/Usuarios", dto);
            Assert.Equal(HttpStatusCode.Created, first.StatusCode);

            var response = await _client.PostAsJsonAsync("/api/Usuarios", dto);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("E-mail já cadastrado", content);
        }

        [Fact]
        public async Task GetUsuario_NotFound()
        {
            var response = await _client.GetAsync("/api/Usuarios/9999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutUsuario_AtualizaComSucesso()
        {
            var dto = new UsuarioRequestDto { Email = "tutor1atualizado@exemplo.com", Senha = "nova", TipoUsuario = "TUTOR" };
            var response = await _client.PutAsJsonAsync("/api/Usuarios/1", dto);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var get = await _client.GetAsync("/api/Usuarios/1");
            var usuario = await get.Content.ReadFromJsonAsync<UsuarioResponseDto>();
            Assert.Equal(dto.Email, usuario.Email);
        }

        [Fact]
        public async Task PutUsuario_TipoInvalido_RetornaBadRequest()
        {
            var dto = new UsuarioRequestDto { Email = "algo@exemplo.com", Senha = "x", TipoUsuario = "INVALID" };
            var response = await _client.PutAsJsonAsync("/api/Usuarios/1", dto);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Tipo de usuário inválido", content);
        }

        [Fact]
        public async Task DeleteUsuario_CriaEDeletaComSucesso()
        {
            // cria
            var dto = new UsuarioRequestDto { Email = "temp@exemplo.com", Senha = "x", TipoUsuario = "TUTOR" };
            var post = await _client.PostAsJsonAsync("/api/Usuarios", dto);
            var created = await post.Content.ReadFromJsonAsync<UsuarioResponseDto>();

            // deleta
            var del = await _client.DeleteAsync($"/api/Usuarios/{created.IdUsuario}");
            Assert.Equal(HttpStatusCode.NoContent, del.StatusCode);

            var get = await _client.GetAsync($"/api/Usuarios/{created.IdUsuario}");
            Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
        }

        [Fact]
        public async Task DeleteUsuario_NotFound()
        {
            var del = await _client.DeleteAsync("/api/Usuarios/9999");
            Assert.Equal(HttpStatusCode.NotFound, del.StatusCode);
        }
    }
}
