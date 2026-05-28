# Sistema Acadêmico API

Projeto desenvolvido para a avaliação A2-2 da disciplina de Tópicos Especiais de Sistemas.

A aplicação consiste em uma Web API REST desenvolvida utilizando ASP.NET Core Minimal API, Entity Framework Core e banco de dados SQLite para persistência de dados.

O sistema permite o gerenciamento de alunos, cursos e matrículas, realizando operações completas de CRUD (Create, Read, Update e Delete), além de aplicar regras de negócio relacionadas ao sistema acadêmico.

---

# Integrantes

* Lucas Ito 
* Rafael Moritz
* Eduardo Alan
* Francisco Canindé
  
---

# Objetivo do Projeto

Desenvolver uma API simples utilizando:

* Minimal API
* REST
* JSON
* Entity Framework Core
* SQLite

O projeto possui persistência em banco de dados utilizando SQLite e migrations com Entity Framework.

---

# Estrutura do Projeto

```txt
SistemaAcademico/
│
├── Data/
│   └── AppDbContext.cs
│
├── Models/
│   ├── Aluno.cs
│   ├── Curso.cs
│   └── Matricula.cs
│
├── Migrations/
│
├── Program.cs
├── sistema.db
├── README.md
└── SistemaAcademico.csproj
```

---

# Entidades do Sistema

## Aluno

Responsável pelo armazenamento dos dados dos alunos cadastrados.

Campos:

* Id
* Nome
* Email
* Matricula
* DataNascimento

---

## Curso

Responsável pelo gerenciamento dos cursos disponíveis.

Campos:

* Id
* Nome
* Professor
* CargaHoraria

---

## Matrícula

Classe de relacionamento entre Aluno e Curso.

Campos:

* Id
* AlunoId
* CursoId
* DataMatricula
* Status

Relacionamento:

* Um aluno pode possuir várias matrículas
* Um curso pode possuir vários alunos

---

# Regra de Negócio

O sistema possui validação para impedir que um aluno seja matriculado duas vezes no mesmo curso.

Caso uma matrícula duplicada seja enviada, a API retorna erro de validação.

---

# Funcionalidades Implementadas

## CRUD de Alunos

* Cadastrar aluno
* Listar alunos
* Buscar aluno por ID
* Atualizar aluno
* Remover aluno

---

## CRUD de Cursos

* Cadastrar curso
* Listar cursos
* Buscar curso por ID
* Atualizar curso
* Remover curso

---

## CRUD de Matrículas

* Realizar matrícula
* Listar matrículas
* Buscar matrícula por ID
* Atualizar matrícula
* Cancelar matrícula

---

# Tecnologias Utilizadas

* ASP.NET Core
* Minimal API
* Entity Framework Core
* SQLite
* JSON
* REST API

---

# Persistência de Dados

O sistema utiliza:

* SQLite como banco de dados
* Entity Framework Core para persistência
* Migrations para criação e atualização do banco

---

# Como Executar o Projeto

## 1. Clonar o repositório

```bash
git clone LINK_DO_REPOSITORIO
```

---

## 2. Acessar a pasta do projeto

```bash
cd SistemaAcademico
```

---

## 3. Restaurar dependências

```bash
dotnet restore
```

---

## 4. Criar o banco de dados

```bash
dotnet ef database update
```

---

## 5. Executar o projeto

```bash
dotnet run
```

---

# Endpoints Principais

## Alunos

```http
GET /alunos
GET /alunos/{id}
POST /alunos
PUT /alunos/{id}
DELETE /alunos/{id}
```

---

## Cursos

```http
GET /cursos
GET /cursos/{id}
POST /cursos
PUT /cursos/{id}
DELETE /cursos/{id}
```

---

## Matrículas

```http
GET /matriculas
GET /matriculas/{id}
POST /matriculas
PUT /matriculas/{id}
DELETE /matriculas/{id}
```

---

# Exemplo de JSON

## Cadastro de Aluno

```json
{
  "nome": "Lucas",
  "email": "lucas@email.com",
  "matricula": "2026001",
  "dataNascimento": "2005-08-15"
}
```

---

# Observações

O projeto foi desenvolvido com foco no aprendizado de:

* APIs REST
* Persistência de dados
* Relacionamento entre entidades
* Organização de projetos em .NET
* Manipulação de JSON
* Utilização de Minimal APIs
