using Microsoft.Extensions.Logging;
using TorneioSC.Application.Services.Util;
using TorneioSC.Domain.Adapters;
using TorneioSC.Domain.Models;
using TorneioSC.Domain.Services;
using TorneioSC.Exception.ExceptionBase.ExceptionFederacao;

namespace TorneioSC.Application.Services
{
    public class FederacaoService : IFederacaoService
    {
        private readonly IFederacaoSqlReadAdapter _federacaoSqlAdapter;
        private readonly ILogger<FederacaoService> _logger;

        public FederacaoService(
            IFederacaoSqlReadAdapter federacaoSqlAdapter,
            ILogger<FederacaoService> logger)
        {
            _federacaoSqlAdapter = federacaoSqlAdapter ?? throw new ArgumentNullException(nameof(federacaoSqlAdapter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Federacao>> ObterFederacaoAsync()
        {
            return await _federacaoSqlAdapter.ObterFederacaoAsync();
        }

        public async Task<Federacao?> ObterFederacaoPorIdAsync(int federacaoId)
        {
            if (federacaoId <= 0)
                throw new ArgumentException("ID da federação inválido", nameof(federacaoId));

            return await _federacaoSqlAdapter.ObterFederacaoPorIdAsync(federacaoId);
        }

        public async Task<Federacao?> ObterPorCnpjAsync(string Cnpj)
        {
            if (string.IsNullOrWhiteSpace(Cnpj))
                throw new ArgumentException("CNPJ não pode ser vazio", nameof(Cnpj));

            return await _federacaoSqlAdapter.ObterPorCnpjAsync(Cnpj.Trim());
        }

        public async Task<int> PostFederacaoAsync(Federacao federacao, int usuarioLogadoId)
        {
            var erros = ValidarFederacao(federacao);
            if (erros.Any())
                throw new ValidacaoFederacaoException(erros);

            var novaFederacao = PrepararCriarFederacao(federacao, usuarioLogadoId);

            if (await _federacaoSqlAdapter.ObterPorCnpjAsync(novaFederacao.Cnpj) != null)
                throw new CnpjEmUsoException(federacao.Cnpj);

            
            return await _federacaoSqlAdapter.PostFederacaoAsync(novaFederacao);
        }

        public async Task<int> PutFederacaoAsync(Federacao federacao, int usuarioLogadoId)
        {
            var erros = ValidarFederacao(federacao);
            if (erros.Any())
                throw new ValidacaoFederacaoException(erros);

            //if (await _federacaoSqlAdapter.ObterPorCnpjUpdateAsync(federacao.Cnpj, federacao.FederacaoId) != null)
            //    throw new CnpjEmUsoException(federacao.Cnpj);

            var federacaoAtualizada = PrepararAtualizarFederacao(federacao, usuarioLogadoId);
            return await _federacaoSqlAdapter.PutFederacaoAsync(federacaoAtualizada);
        }

        public async Task<bool> DeleteFederacaoPorIdAsync(int federacaoId)
        {
            if (federacaoId <= 0)
                throw new ArgumentException("ID da federação inválido", nameof(federacaoId));

            return await _federacaoSqlAdapter.DeleteFederacaoPorIdAsync(federacaoId);
        }
        private List<string> ValidarFederacao(Federacao federacao)
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(federacao.Nome))
                erros.Add("Nome é obrigatório");
            else if (federacao.Nome.Length > 150)
                erros.Add("Nome não pode exceder 150 caracteres");

            //if (string.IsNullOrWhiteSpace(federacao.Cnpj))
            //    erros.Add("CNPJ é obrigatório");
            //else if (!IsValidCNPJ(federacao.Cnpj))
            //    erros.Add("CNPJ em formato inválido");

            if (string.IsNullOrWhiteSpace(federacao.Email))
                erros.Add("Email é obrigatório");
            else if (!IsValidEmail(federacao.Email))
                erros.Add("Email em formato inválido");
            // Validação dos telefones
            if (federacao.Telefones == null || !federacao.Telefones.Any())
            {
                erros.Add("Pelo menos um telefone é obrigatório");
            }
            else
            {
                int index = 1;
                foreach (var telefone in federacao.Telefones)
                {
                    if (string.IsNullOrWhiteSpace(telefone.NumeroTelefone))
                        erros.Add($"Telefone #{index} está vazio");
                    else if (!IsValidTelefone(telefone.NumeroTelefone!))
                        erros.Add($"Telefone #{index} está em formato inválido");

                    index++;
                }
            }

            return erros;
        }

        private Federacao PrepararCriarFederacao(Federacao federacao, int usuarioLogadoId)
        {
            return new Federacao
            {
                Nome = federacao.Nome.Trim(),
                Cnpj = Recursos.RemoverMascaraCNPJ(federacao.Cnpj.Trim()),
                Email = federacao.Email.Trim().ToLower(),
                DataFundacao = federacao.DataFundacao,
                MunicipioId = federacao.MunicipioId,
                Site = federacao.Site.Trim(),
                Portaria = federacao.Portaria.Trim(),
                Ativo = true,
                UsuarioInclusaoId = federacao.UsuarioInclusaoId,
                DataInclusao = DateTime.Now,
                NaturezaOperacao = "I",
                UsuarioOperacaoId = federacao.UsuarioInclusaoId,
                DataOperacao = DateTime.Now,

                // Telefones preparados
                Telefones = federacao.Telefones.Select(t => new Telefone
                {
                    NumeroTelefone = Recursos.RemoverMascaraTelefone(t.NumeroTelefone?.Trim()),
                    TipoTelefoneId = t.TipoTelefoneId,
                    Ativo = true,
                    UsuarioInclusaoId = t.UsuarioInclusaoId,
                    DataInclusao = DateTime.Now,
                    NaturezaOperacao = "I",
                    UsuarioOperacaoId = t.UsuarioInclusaoId,
                    DataOperacao = DateTime.Now
                }).ToList(),

                // Endereços preparados
                Enderecos = federacao.Enderecos.Select(e => new Endereco
                {
                    Logradouro = e.Logradouro.Trim(),
                    Numero = e.Numero.Trim(),
                    Complemento = e.Complemento?.Trim(),
                    Cep = Recursos.RemoverMascaraTelefone(e.Cep?.Trim()),
                    Bairro = e.Bairro?.Trim(),
                    Ativo = true,
                    UsuarioInclusaoId = e.UsuarioInclusaoId,
                    DataInclusao = DateTime.Now,
                    NaturezaOperacao = "I",
                    UsuarioOperacaoId = e.UsuarioInclusaoId,
                    DataOperacao = DateTime.Now
                }).ToList()
            };
        }


        private Federacao PrepararAtualizarFederacao(Federacao federacao, int usuarioLogadoId)
        {
            return new Federacao
            {
                FederacaoId = federacao.FederacaoId,
                Nome = federacao.Nome.Trim(),
                Cnpj = Recursos.RemoverMascaraCNPJ(federacao.Cnpj.Trim()),
                Email = federacao.Email.Trim().ToLower(),
                DataFundacao = federacao.DataFundacao,
                MunicipioId = federacao.MunicipioId,
                Site = federacao.Site.Trim(),
                Portaria = federacao.Portaria.Trim(),
                Ativo = true,                
                NaturezaOperacao = "A",
                UsuarioOperacaoId = federacao.UsuarioOperacaoId,
                DataOperacao = DateTime.Now,

                // Telefones preparados
                Telefones = federacao.Telefones.Select(t => new Telefone
                {
                    NumeroTelefone = Recursos.RemoverMascaraTelefone(t.NumeroTelefone?.Trim()),
                    TipoTelefoneId = t.TipoTelefoneId,
                    Ativo = true,
                    UsuarioInclusaoId = t.UsuarioOperacaoId,
                    DataInclusao = DateTime.Now,
                    NaturezaOperacao = "I",
                    UsuarioOperacaoId = t.UsuarioOperacaoId,
                    DataOperacao = DateTime.Now
                }).ToList(),

                // Endereços preparados
                Enderecos = federacao.Enderecos.Select(e => new Endereco
                {
                    Logradouro = e.Logradouro.Trim(),
                    Numero = e.Numero.Trim(),
                    Complemento = e.Complemento?.Trim(),
                    Cep = Recursos.RemoverMascaraTelefone(e.Cep?.Trim()),
                    Bairro = e.Bairro?.Trim(),
                    Ativo = true,
                    UsuarioInclusaoId = e.UsuarioInclusaoId,
                    DataInclusao = DateTime.Now,
                    NaturezaOperacao = "I",
                    UsuarioOperacaoId = e.UsuarioOperacaoId,
                    DataOperacao = DateTime.Now
                }).ToList()
            };
        }

        private bool IsValidCNPJ(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            // Remove . / - e espaços
            string cleanedCnpj = Recursos.RemoverMascaraCNPJ(cnpj);

            return cleanedCnpj.Length == 14 && long.TryParse(cleanedCnpj, out _);
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private bool IsValidTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return false;

            // Remove tudo que não for dígito
            string cleanedTelefone = Recursos.RemoverMascaraTelefone(telefone);

            // Telefone deve ter 10 (fixo) ou 11 (celular) dígitos com DDD
            return cleanedTelefone.Length == 10 || cleanedTelefone.Length == 11;
        }

    }
}