using AutoMapper;
using TorneioSC.Domain.Models;
using TorneioSC.WebApi.Dtos.AcademiaDtos;
using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.EstadoDtos;
using TorneioSC.WebApi.Dtos.FederacaoDtos;
using TorneioSC.WebApi.Dtos.MunicipioDtos;
using TorneioSC.WebApi.Dtos.PerfilDtos;
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
            #region Usuário
            // Mapeamentos usuario
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
            // Mapeamentos para Federação
            CreateMap<Federacao, FederacaoDto>()
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones))
                .ForMember(dest => dest.Municipio, opt => opt.MapFrom(src => src.Municipio))
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
            // Mapeamentos para operações CRUD (se necessário)
            CreateMap<FederacaoPostDto, Federacao>().ReverseMap();
            CreateMap<FederacaoPutDto, Federacao>()
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones))
                .ReverseMap();

            #endregion

            #region Endereço
            // Mapeamentos para Endereço
            CreateMap<Endereco, EnderecoDto>().ReverseMap();
            CreateMap<EnderecoPostDto, Endereco>().ReverseMap();
            CreateMap<EnderecoPutDto, Endereco>().ReverseMap();

            #endregion

            #region Telefone
            // Mapeamentos para Telefone
            CreateMap<TelefonePostDto, Telefone>().ReverseMap();
            CreateMap<TelefonePutDto, Telefone>().ReverseMap();

            CreateMap<Telefone, TelefoneDto>()
                .ForMember(dest => dest.TipoTelefone, opt => opt.MapFrom(src => src.TipoTelefone))
                .ReverseMap();

            // Mapeamentos para TipoTelefone
            CreateMap<TipoTelefone, TipoTelefoneDto>().ReverseMap();

            #endregion

            #region Municipio
            // Mapeamentos para Municipio
            CreateMap<Municipio, MunicipioDto>()
                .ReverseMap();

            #endregion

            #region Estado
            // Mapeamentos para Estado
            CreateMap<Estado, EstadoDto>()
                .ForMember(dest => dest.Municipios, opt => opt.MapFrom(src => src.Municipios))
                .ReverseMap();

            #endregion

            #region Academia
            // Mapeamentos para Academia
            CreateMap<Academia, AcademiaDto>()
                .ForMember(dest => dest.Federacao, opt => opt.MapFrom(src => src.Federacao))
                .ForMember(dest => dest.Municipio, opt => opt.MapFrom(src => src.Municipio))
                .ForMember(dest => dest.UsuarioInclusao, opt => opt.MapFrom(src => src.UsuarioInclusao))
                .ForMember(dest => dest.UsuarioOperacao, opt => opt.MapFrom(src => src.UsuarioOperacao))
                .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones))
                .ForMember(dest => dest.Atletas, opt => opt.MapFrom(src => src.Atletas))
                .ForMember(dest => dest.Torneios, opt => opt.MapFrom(src => src.Torneios))
                .ForMember(dest => dest.Pontuacoes, opt => opt.MapFrom(src => src.Pontuacoes))
                .ReverseMap();

            #endregion


            #region Academia
            // Mapeamentos para Torneiro
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

        }
    }
}