using AutoMapper;
using TorneioSC.Application.Services.Util;
using TorneioSC.Domain.Models;
using TorneioSC.WebApi.Dtos.AcademiaDtos;
using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.EstadoDtos;
using TorneioSC.WebApi.Dtos.FederacaoDtos;
using TorneioSC.WebApi.Dtos.MunicipioDtos;
using TorneioSC.WebApi.Dtos.PerfilDtos;
using TorneioSC.WebApi.Dtos.RedeSocialDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;
using TorneioSC.WebApi.Dtos.TipoTelefoneDtos;
using TorneioSC.WebApi.Dtos.TorneioDtos;
using TorneioSC.WebApi.Dtos.UsuarioDtos;

namespace TorneioSC.WebApi
{
    public class WebApiMapperProfile : Profile
    {
        public WebApiMapperProfile()
        {
            // Funções auxiliares para limpeza
            Func<string, string> trim = s => (s ?? string.Empty).Trim();
            Func<string, string> trimToLower = s => (s ?? string.Empty).Trim().ToLower();

            // Evita passar null para os métodos de Recursos
            Func<string, string> removerCpf = s => Recursos.RemoverMascaraCPF((s ?? string.Empty).Trim());
            Func<string, string> removerCnpj = s => Recursos.RemoverMascaraCNPJ((s ?? string.Empty).Trim());
            Func<string, string> removerTelefone = s => Recursos.RemoverMascaraTelefone((s ?? string.Empty).Trim());
            Func<string, string> removerCep = s => Recursos.RemoverMascaraCep((s ?? string.Empty).Trim());

            #region Usuário
            CreateMap<UsuarioDto, Usuario>().ReverseMap();
            CreateMap<UsuarioPostDto, Usuario>().ReverseMap();
            CreateMap<UsuarioPutDto, Usuario>().ReverseMap();
            #endregion

            #region Perfil
            CreateMap<Perfil, PerfilDto>()
                .ForMember(dest => dest.Usuarios, opt => opt.MapFrom(src => src.Usuarios))
                .ReverseMap();
            #endregion

            #region Federação
            CreateMap<Federacao, FederacaoDto>()
                .ForMember(dest => dest.Municipio, opt => opt.MapFrom(src => src.Municipio))
                .ForMember(dest => dest.UsuarioInclusao, opt => opt.MapFrom(src => src.UsuarioInclusao))
                .ForMember(dest => dest.UsuarioOperacao, opt => opt.MapFrom(src => src.UsuarioOperacao))
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones))
                .ForMember(dest => dest.FederacaoRedeSociais, opt => opt.MapFrom(src => src.FederacaoRedeSociais))
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones))
                .ReverseMap();

            CreateMap<Municipio, FederacaoMunicipioDto>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
                .ReverseMap();

            CreateMap<Estado, FederacaoEstadoDto>()
                .ReverseMap();

            CreateMap<FederacaoEndereco, FederacaoEnderecoDto>()
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.Endereco))
                .ReverseMap();

            CreateMap<FederacaoTelefone, FederacaoTelefoneDto>()
                .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => src.Telefone))
                .ReverseMap();

            CreateMap<Federacao, FederacaoCompletaDto>()
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones));

            // ✅ FederacaoPostDto → Federacao (com limpeza e campos de auditoria)
            CreateMap<FederacaoPostDto, Federacao>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => trim(src.Nome)))
                .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => removerCnpj(src.Cnpj)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => trimToLower(src.Email)))
                .ForMember(dest => dest.Site, opt => opt.MapFrom(src => trim(src.Site)))
                .ForMember(dest => dest.Portaria, opt => opt.MapFrom(src => trim(src.Portaria)))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.NaturezaOperacao, opt => opt.MapFrom(src => "I"))
                .ForMember(dest => dest.UsuarioInclusaoId, opt => opt.MapFrom(src => src.UsuarioInclusaoId))
                .ForMember(dest => dest.UsuarioOperacaoId, opt => opt.MapFrom(src => src.UsuarioInclusaoId))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.DataOperacao, opt => opt.MapFrom(src => DateTime.Now))

                // Relacionamentos
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones))
                .ForMember(dest => dest.FederacaoRedeSociais, opt => opt.MapFrom(src => src.RedesSociais))

                // Ignorar navegação
                .ForMember(dest => dest.Municipio, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioInclusao, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioOperacao, opt => opt.Ignore());

            // ✅ FederacaoPutDto → Federacao (com limpeza e atualização)
            CreateMap<FederacaoPutDto, Federacao>()
                .ForMember(dest => dest.FederacaoId, opt => opt.MapFrom(src => src.FederacaoId))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => trim(src.Nome)))
                .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => removerCnpj(src.Cnpj)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => trimToLower(src.Email)))
                .ForMember(dest => dest.Site, opt => opt.MapFrom(src => trim(src.Site)))
                .ForMember(dest => dest.DataFundacao, opt => opt.MapFrom(src => src.DataFundacao))
                .ForMember(dest => dest.Portaria, opt => opt.MapFrom(src => trim(src.Portaria)))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo))
                .ForMember(dest => dest.MunicipioId, opt => opt.MapFrom(src => src.MunicipioId))
                .ForMember(dest => dest.UsuarioOperacaoId, opt => opt.MapFrom(src => src.UsuarioAlteracaoId))
                .ForMember(dest => dest.DataOperacao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.NaturezaOperacao, opt => opt.MapFrom(src => "A"))

                // Relacionamentos
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones))
                .ForMember(dest => dest.FederacaoRedeSociais, opt => opt.MapFrom(src => src.RedesSociais))

                // Ignorar navegação
                .ForMember(dest => dest.Municipio, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioInclusao, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioOperacao, opt => opt.Ignore());
            // ✅ FederacaoRedeSocialPostDto → FederacaoRedeSocial
            CreateMap<RedeSocialPostDto, FederacaoRedeSocial>()
                .ForMember(dest => dest.RedeSocialId, opt => opt.MapFrom(src => src.RedeSocialId))
                .ForMember(dest => dest.PerfilUrl, opt => opt.MapFrom(src => trim(src.PerfilUrl)))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.NaturezaOperacao, opt => opt.MapFrom(src => "I"))
                .ForMember(dest => dest.UsuarioInclusaoId, opt => opt.MapFrom(src => src.UsuarioInclusaoId))
                .ForMember(dest => dest.UsuarioOperacaoId, opt => opt.MapFrom(src => src.UsuarioInclusaoId))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.DataOperacao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FederacaoId, opt => opt.Ignore())
                .ForMember(dest => dest.RedeSocial, opt => opt.Ignore());

            // ✅ FederacaoRedeSocialPutDto → FederacaoRedeSocial
            CreateMap<RedeSocialPutDto, FederacaoRedeSocial>()
                .ForMember(dest => dest.FederacaoRedeSocialId, opt => opt.MapFrom(src => src.FederacaoRedeSocialId))
                .ForMember(dest => dest.RedeSocialId, opt => opt.MapFrom(src => src.RedeSocialId))
                .ForMember(dest => dest.PerfilUrl, opt => opt.MapFrom(src => trim(src.PerfilUrl)))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo))
                .ForMember(dest => dest.UsuarioOperacaoId, opt => opt.MapFrom(src => src.UsuarioAlteracaoId))
                .ForMember(dest => dest.DataOperacao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.NaturezaOperacao, opt => opt.MapFrom(src => "A"))
                .ForMember(dest => dest.FederacaoId, opt => opt.Ignore())
                .ForMember(dest => dest.RedeSocial, opt => opt.Ignore());
            CreateMap<FederacaoRedeSocialDto, FederacaoRedeSocial>()
             .ForMember(dest => dest.RedeSocial, opt => opt.MapFrom(src => src.RedeSocial))
             .ReverseMap();

            #endregion

            #region Endereço
            CreateMap<Endereco, EnderecoDto>().ReverseMap();

            // ✅ EnderecoPostDto → Endereco (com limpeza)
            CreateMap<EnderecoPostDto, Endereco>()
                .ForMember(dest => dest.Logradouro, opt => opt.MapFrom(src => trim(src.Logradouro)))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => trim(src.Numero)))
                .ForMember(dest => dest.Complemento, opt => opt.MapFrom(src => trim(src.Complemento)))
                .ForMember(dest => dest.Cep, opt => opt.MapFrom(src => removerCep(src.Cep)))
                .ForMember(dest => dest.Bairro, opt => opt.MapFrom(src => trim(src.Bairro)))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.NaturezaOperacao, opt => opt.MapFrom(src => "I"))
                .ForMember(dest => dest.UsuarioInclusaoId, opt => opt.MapFrom(src => src.UsuarioInclusaoId))
                .ForMember(dest => dest.UsuarioOperacaoId, opt => opt.MapFrom(src => src.UsuarioInclusaoId))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.DataOperacao, opt => opt.MapFrom(src => DateTime.Now))

                // Ignorar navegação
                .ForMember(dest => dest.UsuarioInclusao, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioOperacao, opt => opt.Ignore())
                .ForMember(dest => dest.Academias, opt => opt.Ignore())
                .ForMember(dest => dest.Federacoes, opt => opt.Ignore());

            // ✅ EnderecoPutDto → Endereco (com limpeza)
            CreateMap<EnderecoPutDto, Endereco>()
                .ForMember(dest => dest.EnderecoId, opt => opt.MapFrom(src => src.EnderecoId))
                .ForMember(dest => dest.Logradouro, opt => opt.MapFrom(src => trim(src.Logradouro)))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => trim(src.Numero)))
                .ForMember(dest => dest.Complemento, opt => opt.MapFrom(src => trim(src.Complemento)))
                .ForMember(dest => dest.Cep, opt => opt.MapFrom(src => removerCep(src.Cep)))
                .ForMember(dest => dest.Bairro, opt => opt.MapFrom(src => trim(src.Bairro)))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo))
                .ForMember(dest => dest.UsuarioOperacaoId, opt => opt.MapFrom(src => src.UsuarioAlteracaoId))
                .ForMember(dest => dest.DataOperacao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.NaturezaOperacao, opt => opt.MapFrom(src => "A"))
                .ForMember(dest => dest.UsuarioInclusaoId, opt => opt.MapFrom(src => src.UsuarioAlteracaoId))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UsuarioInclusao, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioOperacao, opt => opt.Ignore())
                .ForMember(dest => dest.Academias, opt => opt.Ignore())
                .ForMember(dest => dest.Federacoes, opt => opt.Ignore());

            #endregion

            #region Telefone

            // ✅ TelefonePostDto → Telefone (com limpeza)
            CreateMap<TelefonePostDto, Telefone>()
                .ForMember(dest => dest.NumeroTelefone, opt => opt.MapFrom(src => removerTelefone(src.NumeroTelefone)))
                .ForMember(dest => dest.TipoTelefoneId, opt => opt.MapFrom(src => src.TipoTelefoneId))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.NaturezaOperacao, opt => opt.MapFrom(src => "I"))
                .ForMember(dest => dest.UsuarioInclusaoId, opt => opt.MapFrom(src => src.UsuarioInclusaoId))
                .ForMember(dest => dest.UsuarioOperacaoId, opt => opt.MapFrom(src => src.UsuarioInclusaoId))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.DataOperacao, opt => opt.MapFrom(src => DateTime.Now))

                // Ignorar navegação
                .ForMember(dest => dest.TipoTelefone, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioInclusao, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioOperacao, opt => opt.Ignore())
                .ForMember(dest => dest.Academias, opt => opt.Ignore())
                .ForMember(dest => dest.Federacoes, opt => opt.Ignore());

            // ✅ TelefonePutDto → Telefone (com limpeza)
            CreateMap<TelefonePutDto, Telefone>()
                .ForMember(dest => dest.TelefoneId, opt => opt.MapFrom(src => src.TelefoneId))
                .ForMember(dest => dest.NumeroTelefone, opt => opt.MapFrom(src => removerTelefone(src.NumeroTelefone)))
                .ForMember(dest => dest.TipoTelefoneId, opt => opt.MapFrom(src => src.TipoTelefoneId))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo))
                .ForMember(dest => dest.UsuarioOperacaoId, opt => opt.MapFrom(src => src.UsuarioAlteracaoId))
                .ForMember(dest => dest.DataOperacao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.NaturezaOperacao, opt => opt.MapFrom(src => "A"))
                .ForMember(dest => dest.UsuarioInclusaoId, opt => opt.MapFrom(src => src.UsuarioAlteracaoId))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.TipoTelefone, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioInclusao, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioOperacao, opt => opt.Ignore())
                .ForMember(dest => dest.Academias, opt => opt.Ignore())
                .ForMember(dest => dest.Federacoes, opt => opt.Ignore());

            CreateMap<Telefone, TelefoneDto>()
                .ForMember(dest => dest.TipoTelefone, opt => opt.MapFrom(src => src.TipoTelefone))
                .ReverseMap();

            CreateMap<TipoTelefone, TipoTelefoneDto>().ReverseMap();
            #endregion

            #region Municipio
            CreateMap<Municipio, MunicipioDto>().ReverseMap();
            #endregion

            #region Estado
            CreateMap<Estado, EstadoDto>()
                .ForMember(dest => dest.Municipios, opt => opt.MapFrom(src => src.Municipios))
                .ReverseMap();
            #endregion

            #region Academia

            CreateMap<Academia, AcademiaDto>()
                .ForMember(dest => dest.Federacao, opt => opt.MapFrom(src => src.Federacao))
                .ForMember(dest => dest.Municipio, opt => opt.MapFrom(src => src.Municipio))
                .ForMember(dest => dest.UsuarioInclusao, opt => opt.MapFrom(src => src.UsuarioInclusao))
                .ForMember(dest => dest.UsuarioOperacao, opt => opt.MapFrom(src => src.UsuarioOperacao))
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones))
                .ForMember(dest => dest.AcademiaRedeSociais, opt => opt.MapFrom(src => src.AcademiaRedeSociais))
                .ForMember(dest => dest.Atletas, opt => opt.MapFrom(src => src.Atletas))
                .ForMember(dest => dest.Torneios, opt => opt.MapFrom(src => src.Torneios))
                .ForMember(dest => dest.Pontuacoes, opt => opt.MapFrom(src => src.Pontuacoes))
                .ReverseMap();

            // ✅ AcademiaPostDto → Academia(com limpeza e campos de auditoria)
            CreateMap<AcademiaPostDto, Academia>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => trim(src.Nome)))
                .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => removerCnpj(src.Cnpj)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => trimToLower(src.Email)))
                .ForMember(dest => dest.ResponsavelNome, opt => opt.MapFrom(src => trim(src.ResponsavelNome)))
                .ForMember(dest => dest.ResponsavelCpf, opt => opt.MapFrom(src => removerCpf(src.ResponsavelCpf)))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => trim(src.LogoUrl)))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => trim(src.Descricao)))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true)) // Sempre ativo na criação
                .ForMember(dest => dest.NaturezaOperacao, opt => opt.MapFrom(src => "I"))
                .ForMember(dest => dest.UsuarioInclusaoId, opt => opt.MapFrom(src => src.UsuarioInclusaoId))
                .ForMember(dest => dest.UsuarioOperacaoId, opt => opt.MapFrom(src => src.UsuarioInclusaoId)) // Mesmo usuário no início
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.DataOperacao, opt => opt.MapFrom(src => DateTime.Now))

                // Relacionamentos
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones))
                .ForMember(dest => dest.AcademiaRedeSociais, opt => opt.MapFrom(src => src.RedesSociais))

                // Ignorar navegação (será carregada depois)
                .ForMember(dest => dest.Federacao, opt => opt.Ignore())
                .ForMember(dest => dest.Municipio, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioInclusao, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioOperacao, opt => opt.Ignore());


            // ✅ AcademiaPutDto → Academia (com limpeza)
            CreateMap<AcademiaPutDto, Academia>()
                .ForMember(dest => dest.AcademiaId, opt => opt.MapFrom(src => src.AcademiaId))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => trim(src.Nome)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => trimToLower(src.Email)))
                .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => removerCnpj(src.Cnpj)))
                .ForMember(dest => dest.MunicipioId, opt => opt.MapFrom(src => src.MunicipioId))
                .ForMember(dest => dest.FederacaoId, opt => opt.MapFrom(src => src.FederacaoId))
                .ForMember(dest => dest.ResponsavelNome, opt => opt.MapFrom(src => trim(src.ResponsavelNome)))
                .ForMember(dest => dest.ResponsavelCpf, opt => opt.MapFrom(src => removerCpf(src.ResponsavelCpf)))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => trim(src.LogoUrl)))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => trim(src.Descricao)))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo))
                .ForMember(dest => dest.UsuarioOperacaoId, opt => opt.MapFrom(src => src.UsuarioAlteracaoId))
                .ForMember(dest => dest.DataOperacao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.NaturezaOperacao, opt => opt.MapFrom(src => "A"))
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones))
                .ForMember(dest => dest.AcademiaRedeSociais, opt => opt.MapFrom(src => src.RedesSociais))
                .ForMember(dest => dest.UsuarioInclusao, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioOperacao, opt => opt.Ignore())
                .ForMember(dest => dest.Federacao, opt => opt.Ignore())
                .ForMember(dest => dest.Municipio, opt => opt.Ignore());

            CreateMap<AcademiaRedeSocialDto, AcademiaRedeSocial>()
              .ForMember(dest => dest.RedeSocial, opt => opt.MapFrom(src => src.RedeSocial))
              .ReverseMap();
            #endregion

            #region Torneio
            CreateMap<Torneio, TorneioDto>()
                .ForMember(dest => dest.Municipio, opt => opt.MapFrom(src => src.Municipio))
                .ForMember(dest => dest.UsuarioInclusao, opt => opt.MapFrom(src => src.UsuarioInclusao))
                .ForMember(dest => dest.UsuarioOperacao, opt => opt.MapFrom(src => src.UsuarioOperacao))
                .ForMember(dest => dest.Academias, opt => opt.MapFrom(src => src.Academias))
                .ForMember(dest => dest.Inscricoes, opt => opt.MapFrom(src => src.Inscricoes))
                .ForMember(dest => dest.Chaveamentos, opt => opt.MapFrom(src => src.Chaveamentos))
                .ForMember(dest => dest.EquipePontuacoes, opt => opt.MapFrom(src => src.EquipePontuacoes))
                .ForMember(dest => dest.EstatisticasPosEvento, opt => opt.MapFrom(src => src.EstatisticasPosEvento))
                .ForMember(dest => dest.EstatisticasPreEvento, opt => opt.MapFrom(src => src.EstatisticasPreEvento))
                .ForMember(dest => dest.Eventos, opt => opt.MapFrom(src => src.Eventos))
                .ReverseMap();
            #endregion

            #region Rede Social
            // ✅ Correto: mapeia para AcademiaRedeSocial, não para RedeSocial
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
            CreateMap<RedeSocialPostDto, RedeSocial>().ReverseMap();

            // ❌ Removido: CreateMap<RedeSocialPutDto, RedeSocial>() → errado!

            // ✅ Correto: RedeSocialPutDto → AcademiaRedeSocial
            CreateMap<RedeSocialPutDto, AcademiaRedeSocial>()
                .ForMember(dest => dest.AcademiaRedeSocialId, opt => opt.MapFrom(src => src.AcademiaRedeSocialId))
                .ForMember(dest => dest.RedeSocialId, opt => opt.MapFrom(src => src.RedeSocialId))
                .ForMember(dest => dest.PerfilUrl, opt => opt.MapFrom(src => trim(src.PerfilUrl)))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.Ativo))
                .ForMember(dest => dest.UsuarioOperacaoId, opt => opt.MapFrom(src => src.UsuarioAlteracaoId))
                .ForMember(dest => dest.DataOperacao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.NaturezaOperacao, opt => opt.MapFrom(src => "A"))
                .ForMember(dest => dest.UsuarioInclusaoId, opt => opt.MapFrom(src => src.UsuarioAlteracaoId))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.AcademiaId, opt => opt.Ignore()) // Será preenchido no serviço
                .ForMember(dest => dest.RedeSocial, opt => opt.Ignore());

            // ✅ Para criação (POST) - se necessário
            CreateMap<RedeSocialPostDto, AcademiaRedeSocial>()
                .ForMember(dest => dest.RedeSocialId, opt => opt.MapFrom(src => src.RedeSocialId))
                .ForMember(dest => dest.PerfilUrl, opt => opt.MapFrom(src => trim(src.PerfilUrl)))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.UsuarioInclusaoId, opt => opt.MapFrom(src => src.UsuarioInclusaoId))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.NaturezaOperacao, opt => opt.MapFrom(src => "I"))
                .ForMember(dest => dest.UsuarioOperacaoId, opt => opt.MapFrom(src => src.UsuarioInclusaoId))
                .ForMember(dest => dest.DataOperacao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.AcademiaId, opt => opt.Ignore())
                .ForMember(dest => dest.RedeSocial, opt => opt.Ignore());
            #endregion

        }
    }
}