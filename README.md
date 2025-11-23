# 🥋 TorneioSC - Sistema de Gerenciamento de Torneios de Karatê

API para gerenciamento de torneios de karatê, incluindo cadastro de academias, atletas, categorias, chaveamentos e resultados. Desenvolvida em **C#** com **.NET Core** e **SQL Server**.

Sistema completo para gerenciamento de torneios de karatê, desenvolvido em arquitetura limpa com .NET Core, incluindo cadastro de academias, atletas, federações, inscrições e resultados competitivos.

## 📋 Índice
- [🏗️ Arquitetura](#️-arquitetura)
- [🚀 Funcionalidades](#-funcionalidades)
- [💻 Tecnologias](#-tecnologias)
- [📁 Estrutura do Projeto](#-estrutura-do-projeto)
- [⚙️ Configuração](#️-configuração)
- [🔐 Autenticação](#-autenticação)
- [📚 Documentação da API](#-documentação-da-api)
- [🛣️ Endpoints Principais](#️-endpoints-principais)
- [🧪 Testes](#-testes)
- [🐳 Deploy](#-deploy)

## 🏗️ Arquitetura

O projeto segue os princípios da **Clean Architecture** com separação em camadas:

TorneioSC/<br>
├── 📁 TorneioSC.Domain/ # Entidades e contratos <br>
├── 📁 TorneioSC.Application/ # Lógica de negócio e serviços  <br>
├── 📁 TorneioSC.Infrastructure/ # Implementações externas (SQL Server)  <br>
├── 📁 TorneioSC.WebApi/ # API REST e controllers  <br>
└── 📁 TorneioSC.Exception/ # Exceções personalizadas  <br>

## 🚀 Funcionalidades

### 👥 Gestão de Usuários e Perfis
- Autenticação JWT
- Múltiplos perfis (Admin, Organizador, Árbitro, Atleta)
- Redefinição de senha com token

### 🏢 Gestão de Federações e Academias
- Cadastro completo de federações
- Vinculação de academias às federações
- Gestão de endereços e contatos
- Redes sociais e informações de contato

### 👤 Gestão de Atletas
- Cadastro com dados completos
- Categorização por idade, peso e sexo
- Vinculação à academias
- Histórico de inscrições

### 🏆 Sistema de Torneios
- Criação e gestão de torneios
- Categorias e modalidades (Kata, Kumite)
- Período de inscrições
- Sistema de chaveamento

### 📝 Inscrições e Resultados
- Inscrição de atletas em categorias
- Registro de resultados e pontuações
- Geração de certificados
- Ranking por equipe

### 📊 Relatórios e Estatísticas
- Estatísticas pré-evento (inscrições, categorias)
- Estatísticas pós-evento (medalhas, certificados)
- Pontuação por equipe/academia

## 💻 Tecnologias

- **.NET 6** - Framework principal
- **ASP.NET Core** - API Web
- **Entity Framework Core** - ORM
- **SQL Server** - Banco de dados
- **JWT Bearer** - Autenticação
- **Swagger/OpenAPI** - Documentação
- **AutoMapper** - Mapeamento de DTOs
- **xUnit** - Testes unitários
- **Dapper** - Queries de alto desempenho

## 📁 Estrutura do Projeto

### TorneioSC.Domain

Domain/<br>
├── Models/ # Entidades de negócio<br>
├── Services/ # Interfaces de serviços <br>
├── Dtos/ # Objetos de transferência de dados <br>
└── Adapters/ # Contratos para adaptadores <br>

### TorneioSC.Application

SqlServerAdapter/ <br>
├── Context/ # DbContext do EF Core <br>
├── Entities/ # Entidades do banco <br>
├── Adapters/ # Implementação dos adaptadores <br>
└── Mappings/ # Configurações do EF <br>

### TorneioSC.WebApi

WebApi/ <br>
├── Controllers/ # Controladores da API <br>
├── Dtos/ # DTOs específicos da API <br>
├── Filters/ # Filtros personalizados <br>
├── Middlewares/ # Middlewares customizados <br>
├── Services/ # Serviços da Web API <br>
└── Profiles/ # Perfis do AutoMapper <br>

### TorneioSC.Exception

Exception/ <br>
└── ExceptionBase/ <br>
├── ExceptionUsuario/ # Exceções de usuário <br>
├── ExceptionFederacao/ # Exceções de federação <br>
└── ExceptionPerfil/ # Exceções de perfil <br>

## 📦 Pré-requisitos
- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- [SQL Server 2014+](https://www.microsoft.com/sql-server)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

## ⚙️ Configuração

### Pré-requisitos
- .NET 6.0 SDK
- SQL Server 2014+
- Visual Studio 2022 ou VS Code

### 1. Clone o repositório
```bash
git clone https://github.com/seu-usuario/torneiosc-api.git
cd torneiosc-api

📚 Documentação da API
Acesse a documentação interativa Swagger em:

https://localhost:7001/swagger

