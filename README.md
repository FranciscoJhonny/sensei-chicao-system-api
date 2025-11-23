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

TorneioSC/
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

Domain/
├── Models/ # Entidades de negócio
├── Services/ # Interfaces de serviços
├── Dtos/ # Objetos de transferência de dados
└── Adapters/ # Contratos para adaptadores

### TorneioSC.Application

SqlServerAdapter/
├── Context/ # DbContext do EF Core
├── Entities/ # Entidades do banco
├── Adapters/ # Implementação dos adaptadores
└── Mappings/ # Configurações do EF

### TorneioSC.WebApi

WebApi/
├── Controllers/ # Controladores da API
├── Dtos/ # DTOs específicos da API
├── Filters/ # Filtros personalizados
├── Middlewares/ # Middlewares customizados
├── Services/ # Serviços da Web API
└── Profiles/ # Perfis do AutoMapper

### TorneioSC.Exception

Exception/
└── ExceptionBase/
├── ExceptionUsuario/ # Exceções de usuário
├── ExceptionFederacao/ # Exceções de federação
└── ExceptionPerfil/ # Exceções de perfil

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

