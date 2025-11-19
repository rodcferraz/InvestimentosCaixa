using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControlerTestes.Mapper
{
    public class ClienteMapperErroFake : IClienteMapper
    {
        public Cliente ToBaseEntity(ClienteDTOBaseRequest clienteDto)
        {
            throw new ConvertEnumException(typeof(PerfilRiscoClienteEnum), clienteDto.Liquidez);
        }

        public ClienteDTOResponse ToDtoResponse(Cliente cliente)
        {
            return new ClienteDTOResponse
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
            };
        }

        public List<ClienteDTOResponse> ToDtoResponseList(IEnumerable<Cliente> clientes)
        {
            return clientes.Select(c => new ClienteDTOResponse
            {
                Id = c.Id,
                Nome = c.Nome,
            }).ToList();
        }

        public Cliente ToEntity(ClienteDTOCadastroRequest clienteDto)
        {
            throw new ConvertEnumException(typeof(PerfilRiscoClienteEnum), clienteDto.Liquidez);
        }

        public Cliente ToEntity(ClienteDTORequest clienteDto)
        {
            throw new NotImplementedException();
        }
    }
}
