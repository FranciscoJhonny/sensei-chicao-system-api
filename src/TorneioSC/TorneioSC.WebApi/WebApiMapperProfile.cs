using AutoMapper;
using TorneioSC.Domain.Models;
using TorneioSC.WebApi.Dtos.EnderecoDtos;
using TorneioSC.WebApi.Dtos.FederacaoDtos;
using TorneioSC.WebApi.Dtos.PerfilDtos;
using TorneioSC.WebApi.Dtos.TelefoneDtos;
using TorneioSC.WebApi.Dtos.TipoTelefoneDtos;
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
            CreateMap<FederacaoPutDto, Federacao>().ReverseMap();

            #endregion

            #region Endereço
            // Mapeamentos para Endereço
            CreateMap<Endereco, EnderecoDto>().ReverseMap();
            CreateMap<EnderecoPostDto, Endereco>().ReverseMap();

            #endregion

            #region Telefone
            // Mapeamentos para Telefone
            CreateMap<TelefonePostDto, Telefone>().ReverseMap();
            CreateMap<Telefone, TelefoneDto>()
                .ForMember(dest => dest.TipoTelefone, opt => opt.MapFrom(src => src.TipoTelefone))
                .ReverseMap();

            // Mapeamentos para TipoTelefone
            CreateMap<TipoTelefone, TipoTelefoneDto>().ReverseMap();

            #endregion




        }
    }
}