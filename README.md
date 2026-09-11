# ClyvoVet API - Challenge FIAP 2026

## Descrição do Projeto

A SuperVet API é uma API RESTful desenvolvida em ASP.NET Core para gerenciamento do núcleo clínico do sistema ClyvoVet.

O projeto foi desenvolvido para o Challenge FIAP, permitindo o gerenciamento de usuários, tutores e veterinários, aplicando integração com Oracle Database, Entity Framework Core e documentação OpenAPI.

A partir da Sprint 3, o projeto evoluiu com a adição de monitoramento e observabilidade (Health Checks, Serilog e OpenTelemetry) e testes automatizados (unitários e de integração) seguindo o padrão AAA.

---

## Tecnologias Utilizadas

- ASP.NET Core
- C#
- Entity Framework Core
- Oracle Database
- Oracle.EntityFrameworkCore
- Scalar (OpenAPI)
- Git / GitHub

---

## Estrutura do Projeto

```text
SuperVet
│
├── Controllers
│   ├── UsuariosController.cs
│   ├── TutoresController.cs
│   └── VeterinariosController.cs
│
├── Data
│   └── AppDbContext.cs
│
├── DTOs
│   ├── UsuarioRequestDto.cs
│   ├── UsuarioResponseDto.cs
│   ├── TutorRequestDto.cs
│   ├── TutorResponseDto.cs
│   ├── VeterinarioRequestDto.cs
│   └── VeterinarioResponseDto.cs
│
├── Models
│   ├── Usuario.cs
│   ├── Tutor.cs
│   └── Veterinario.cs
│
├── Services
│   ├── IUsuarioService.cs
│   └── UsuarioService.cs
│
├── Migrations
├── logs
│   └── supervetAAAAMMDD.txt
│
├── SuperVet.Tests
│   ├── Controller
│   │   └── UsuariosController.UnitTests.cs
│   └── Services
│       └── UsuarioService.UnitTests.cs
│
├── SuperVet.Integrations.Tests
│   ├── FactoryFixture
│   │   └── ApiFactoryFixture.cs
│   └── Integration
│       └── SuperVetIntegrationTests.cs
│
├── Program.cs
├── appsettings.json
└── README.md
```
# 🧩 Entidades Implementadas

## 👤 Usuário

Representa a conta principal do sistema.

### Campos

- IdUsuario
- Email
- Senha
- TipoUsuario
- DataCriacao

### Tipos permitidos

- TUTOR
- VETERINARIO

---

## 🧑 Tutor

Representa o responsável pelo pet.

### Campos

- IdTutor
- NomeTutor
- CPF
- Telefone
- DataNascimento
- IdUsuario

---

## 🩺 Veterinário

Representa o profissional veterinário.

### Campos

- IdVeterinario
- Nome
- CRMV
- Telefone
- Especialidade
- DataNascimento
- IdUsuario

---

# 🌐 Rotas da API

## 👤 Usuários

| Método | Endpoint | Descrição |
|----------|----------|----------|
| GET | `/api/usuarios` | Lista todos os usuários |
| GET | `/api/usuarios/{id}` | Busca usuário por ID |
| GET | `/api/usuarios/email/{email}` | Busca usuário por e-mail |
| GET | `/api/usuarios/tipo/{tipo}` | Busca usuários por tipo |
| POST | `/api/usuarios` | Cria usuário |
| PUT | `/api/usuarios/{id}` | Atualiza usuário |
| DELETE | `/api/usuarios/{id}` | Remove usuário |

### Exemplo — Criar Usuário

```json
{
   "email":"usuario@email.com",
   "senha":"123456",
   "tipoUsuario":"TUTOR"
}
```
## 🧑 Tutores

| Método | Endpoint | Descrição |
|----------|----------|----------|
| GET | `/api/tutores` | Lista tutores |
| GET | `/api/tutores/{id}` | Busca tutor por ID |
| GET | `/api/tutores/cpf/{cpf}` | Busca tutor por CPF |
| POST | `/api/tutores` | Cria tutor |
| PUT | `/api/tutores/{id}` | Atualiza tutor |
| DELETE | `/api/tutores/{id}` | Remove tutor |

### Exemplo — Criar Tutor

```json
{
   "nomeTutor":"Lucas Giannini",
   "cpf":"12345678901",
   "telefone":"11999999999",
   "dataNascimento":"2006-05-30",
   "emailUsuario":"usuario@email.com"
}

```

## 🩺 Veterinários

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/veterinarios` | Lista veterinários |
| GET | `/api/veterinarios/{id}` | Busca veterinário por ID |
| GET | `/api/veterinarios/crmv/{crmv}` | Busca veterinário por CRMV |
| GET | `/api/veterinarios/especialidade/{especialidade}` | Busca por especialidade |
| POST | `/api/veterinarios` | Cria veterinário |
| PUT | `/api/veterinarios/{id}` | Atualiza veterinário |
| DELETE | `/api/veterinarios/{id}` | Remove veterinário |

### Exemplo — Criar Veterinário

```http

{
   "nome":"Dra Ana",
   "crmv":"CRMV-SP-12345",
   "telefone":"11999999999",
   "especialidade":"Cirurgião Cardíaco",
   "dataNascimento":"1990-03-20",
   "emailUsuario":"vet@email.com"
}
```
---

# 📌 Regras de Negócio

✅ E-mail único para usuário

✅ Tipos permitidos:

```text
TUTOR
VETERINARIO
```

✅ CPF único para tutor

✅ CRMV único para veterinário

Formato aceito:

```text
CRMV-UF-12345
```

Exemplo:

```text
CRMV-SP-12345
```

✅ Um usuário pode possuir apenas um tutor

✅ Um usuário pode possuir apenas um veterinário

✅ Não é permitido excluir usuários vinculados a tutor ou veterinário

# 📡 Retornos HTTP Utilizados
| Código | Descrição   |
| ------ | ----------- |
| 200    | OK          |
| 201    | Created     |
| 204    | No Content  |
| 400    | Bad Request |
| 404    | Not Found   |

---

# 🗄 Banco de Dados

Banco utilizado:

**Oracle Database**

### Configuração da conexão

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/orcl"
  }
}
```

---

# 🔄 Migrations

Migration criada:

```bash
dotnet ef migrations add InitialMapping
```

Como o banco já existia previamente, a migration foi utilizada apenas para evidenciar o uso do recurso.

---

---

# 📄 Documentação

A documentação da API é gerada automaticamente pelo **Scalar (OpenAPI)**.

Após executar a aplicação pelo Visual Studio, a interface será aberta automaticamente no navegador.

**Finalidade do Scalar:**

✅ Visualizar endpoints disponíveis  
✅ Testar GET / POST / PUT / DELETE  
✅ Visualizar exemplos e parâmetros  
✅ Consultar respostas HTTP  
✅ Navegar pela documentação da API  

---
---

# 🏥 Monitoramento e Observabilidade

## Health Checks

A API expõe dois endpoints de health check:

| Endpoint | Descrição |
|---|---|
| GET `/health` | Retorna `Healthy` ou `Unhealthy` em texto simples |
| GET `/health/detail` | Retorna JSON detalhado com status de cada check |

### Exemplo de resposta — `/health/detail`

```json
{
  "status": "Healthy",
  "checks": [
    {
      "name": "api",
      "status": "Healthy",
      "description": "API está funcionando."
    },
    {
      "name": "oracle-db",
      "status": "Healthy",
      "description": null
    }
  ]
}
```

Os checks verificam:

✅ Disponibilidade geral da API  
✅ Conectividade com o banco de dados Oracle  

---

## Logging Estruturado — Serilog

O Serilog registra todas as requisições e eventos da aplicação em dois destinos:

✅ **Console** — exibido no terminal durante a execução  
✅ **Arquivo** — salvo na pasta `logs/` com rotação diária  

Exemplo de log gerado:

```text
[23:44:34 INF] HTTP GET /health responded 200 in 24.9 ms
[23:44:53 INF] Executed DbCommand (95ms) SELECT * FROM TB_USUARIO
```

Níveis de log configurados:

| Nível | Quando é gerado |
|---|---|
| Information | Requisições, respostas, queries SQL |
| Warning | Redirecionamentos falhos, comportamentos inesperados |
| Error | Exceções não tratadas |

---

## Tracing e Métricas — OpenTelemetry

O OpenTelemetry rastreia requisições entre camadas da aplicação e expõe métricas de desempenho:

✅ **Tracing distribuído** — rastreia o fluxo completo de cada requisição  
✅ **Métricas** — tempo de resposta, taxa de requisições e erros  
✅ **Instrumentação automática** de ASP.NET Core e HttpClient  

---

# 🧪 Testes Automatizados

## Estrutura dos Testes

Os testes estão organizados em dois projetos separados:

| Projeto | Tipo | Descrição |
|---|---|---|
| `SuperVet.Tests` | Unitários | Testa controllers e services isoladamente com Moq e InMemory |
| `SuperVet.Integrations.Tests` | Integração | Testa o fluxo HTTP completo com WebApplicationFactory |

---

## Testes Unitários

Seguem o padrão **AAA (Arrange, Act, Assert)** e utilizam **Moq** para mockar dependências.

**Controller** — verifica se os status HTTP estão corretos:

| Teste | Cenário | Resultado Esperado |
|---|---|---|
| `GetAll_QuandoExistemUsuarios_RetornaOkComLista` | Usuários cadastrados | 200 OK |
| `GetById_QuandoUsuarioExiste_RetornaOkComUsuario` | ID válido | 200 OK |
| `GetById_QuandoUsuarioNaoExiste_RetornaNotFound` | ID inexistente | 404 Not Found |
| `Create_QuandoDadosValidos_RetornaCreated` | Dados válidos | 201 Created |
| `Create_QuandoEmailDuplicado_RetornaBadRequest` | E-mail duplicado | 400 Bad Request |
| `Delete_QuandoExiste_RetornaNoContent` | ID válido | 204 No Content |
| `Delete_QuandoNaoExiste_RetornaNotFound` | ID inexistente | 404 Not Found |

**Service** — verifica as regras de negócio:

| Teste | Cenário | Resultado Esperado |
|---|---|---|
| `CreateAsync_QuandoDadosValidos_RetornaUsuarioCriado` | Dados válidos | Usuário criado |
| `CreateAsync_QuandoEmailDuplicado_LancaInvalidOperationException` | E-mail duplicado | Exception |
| `UpdateAsync_QuandoTipoInvalido_LancaInvalidOperationException` | Tipo inválido | Exception |
| `DeleteAsync_QuandoPossuiTutor_LancaInvalidOperationException` | Usuário com tutor | Exception |
| `DeleteAsync_QuandoNaoPossuiRelacionados_RemoveERetornaTrue` | Sem relacionados | true |

---

## Testes de Integração

Utilizam **WebApplicationFactory** com banco **InMemory** substituindo o Oracle, e **Collection Fixtures** para compartilhar o contexto entre testes.

| Teste | Descrição |
|---|---|
| `Health_RetornaOk` | Verifica se `/health` responde 200 |
| `GetTutores_RetornaOkELista` | Verifica listagem de tutores |
| `PostUsuario_CriaComSucesso` | Cria usuário e verifica 201 |
| `PostUsuario_EmailDuplicado_RetornaBadRequest` | Verifica 400 em email duplicado |
| `GetUsuario_NotFound` | Verifica 404 para ID inexistente |
| `PutUsuario_AtualizaComSucesso` | Atualiza e verifica 204 |
| `PutUsuario_TipoInvalido_RetornaBadRequest` | Verifica 400 em tipo inválido |
| `DeleteUsuario_CriaEDeletaComSucesso` | Cria e deleta verificando 204 |
| `DeleteUsuario_NotFound` | Verifica 404 ao deletar inexistente |

---

## Como Executar os Testes

### Testes Unitários

```bash
cd SuperVet.Tests
dotnet test
```

### Testes de Integração

```bash
cd SuperVet.Integrations.Tests
dotnet test
```

### Todos os testes de uma vez

```bash
cd SuperVet
dotnet test
```

# 📦 Dependências Instaladas

Pacotes utilizados no projeto:
```bash
Microsoft.AspNetCore.OpenApi
Microsoft.EntityFrameworkCore.Design
Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
Oracle.EntityFrameworkCore
Oracle.ManagedDataAccess.Core
Scalar.AspNetCore
Serilog.AspNetCore
Serilog.Sinks.Console
Serilog.Sinks.File
OpenTelemetry.Extensions.Hosting
OpenTelemetry.Instrumentation.AspNetCore
OpenTelemetry.Instrumentation.Http
OpenTelemetry.Exporter.Console
AspNetCore.HealthChecks.Oracle
xunit
Moq
Microsoft.AspNetCore.Mvc.Testing
Microsoft.EntityFrameworkCore.InMemory
```

---

# ▶️ Como Executar

## 1. Clonar repositório

```bash
git clone URL_DO_REPOSITORIO
cd SuperVet
```

## 2. Abrir projeto

Abrir a solução:

```text
SuperVet.sln
```

no **Visual Studio**.

## 3. Configurar conexão Oracle

Editar:

```text
appsettings.json
```

Exemplo:

```json
{
  "ConnectionStrings": {
    "OracleConnection":"User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/orcl"
  }
}
```

## 4. Restaurar dependências

O Visual Studio restaurará automaticamente as dependências ao abrir o projeto.

Caso necessário:

```bash
dotnet restore
```

## 5. Executar aplicação

Clique no botão:

```text
▶ Play / IIS Express
```

A documentação Scalar será aberta automaticamente.

---


