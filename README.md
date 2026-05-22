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

✅ CPF único para tutor

✅ CRMV único para veterinário

Formato aceito:

CRMV-UF-12345

Exemplo:

CRMV-SP-12345

✅ Um usuário pode possuir apenas um tutor

✅ Um usuário pode possuir apenas um veterinário

✅ Não é permitido excluir usuários vinculados a tutor ou veterinário
````
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

# 📄 Documentação OpenAPI

### Scalar

```text
https://localhost:7289/scalar/v1
```

### OpenAPI JSON

```text
https://localhost:7289/openapi/v1.json
```

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

### Instalação das dependências

```bash
dotnet add package Microsoft.AspNetCore.OpenApi

dotnet add package Microsoft.EntityFrameworkCore.Design

dotnet add package Oracle.EntityFrameworkCore

dotnet add package Oracle.ManagedDataAccess.Core

dotnet add package Scalar.AspNetCore
```

# ▶️ Como Executar

## Clonar repositório

```bash
git clone URL_DO_REPOSITORIO
cd Challenge_Sprints1e2
```

## Configurar conexão Oracle

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

## Restaurar dependências

```bash
dotnet restore
```

## Executar aplicação

```bash
dotnet run
```

## Acessar documentação

Scalar:

```text
https://localhost:7289/scalar/v1
```

OpenAPI:

```text
https://localhost:7289/openapi/v1.json
```

# 👥 Integrantes

- **Lucas Barranha Giannini** — RM564508  
- **João Victor Gomes De Souza** — RM560907  
- **Maria Luiza Alves De Aquino** — RM561802
