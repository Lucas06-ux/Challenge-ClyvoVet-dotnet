# ClyvoVet API - Challenge FIAP 2026

## Descrição do Projeto

A ClyvoVet API é uma API RESTful desenvolvida em ASP.NET Core para gerenciamento do núcleo clínico do sistema ClyvoVet.

O projeto foi desenvolvido para o Challenge FIAP, permitindo o gerenciamento de usuários, tutores e veterinários, aplicando integração com Oracle Database, Entity Framework Core e documentação OpenAPI.

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
Challenge_Sprints1e2
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
│   ├── VeterinarioResponseDto.cs
│
├── Models
│   ├── Usuario.cs
│   ├── Tutor.cs
│   └── Veterinario.cs
│
├── Migrations
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

# 📦 Dependências Instaladas

Pacotes utilizados no projeto:

```bash
Microsoft.AspNetCore.OpenApi
Microsoft.EntityFrameworkCore.Design
Oracle.EntityFrameworkCore
Oracle.ManagedDataAccess.Core
Scalar.AspNetCore
```

---

# ▶️ Como Executar

## 1. Clonar repositório

```bash
git clone URL_DO_REPOSITORIO
cd Challenge_Sprints1e2
```

## 2. Abrir projeto

Abrir a solução:

```text
Challenge_Sprints1e2.sln
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

# 👥 Integrantes

- **Lucas Barranha Giannini** — RM564508  
- **João Victor Gomes De Souza** — RM560907  
- **Maria Luiza Alves De Aquino** — RM561802
