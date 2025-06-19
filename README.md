# 🥋 Torneio de Karatê API

API para gerenciamento de torneios de karatê, incluindo cadastro de academias, atletas, categorias, chaveamentos e resultados. Desenvolvida em **C#** com **.NET Core** e **SQL Server**.

## 📋 Índice
- [Funcionalidades](#-funcionalidades)
- [Tecnologias](#-tecnologias)
- [Pré-requisitos](#-pré-requisitos)
- [Configuração](#-configuração)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Endpoints](#-endpoints)
- [Autenticação](#-autenticação)
- [Variáveis de Ambiente](#-variáveis-de-ambiente)
- [Deploy](#-deploy)
- [Testes](#-testes)
- [Licença](#-licença)

## 🚀 Funcionalidades
- ✅ Cadastro de academias e federações
- ✅ Gerenciamento de atletas com categorias por idade/peso
- ✅ Inscrição em torneios (federativos ou open)
- ✅ Sistema de chaveamento automático
- ✅ Cálculo de pontuação por equipe
- ✅ Geração de certificados digitais
- ✅ Relatórios pré e pós-evento

## 💻 Tecnologias
- **.NET Core 6+**
- **Entity Framework Core** (ORM)
- **SQL Server** (Banco de dados)
- **Swagger** (Documentação de API)
- **xUnit** (Testes unitários)
- **Dapper** (Para queries complexas)
- **JWT** (Autenticação)

## 📦 Pré-requisitos
- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- [SQL Server 2014+](https://www.microsoft.com/sql-server)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

## ⚙ Configuração
1. Clone o repositório:
```bash
git clone https://github.com/seu-usuario/karate-tournament-api.git
