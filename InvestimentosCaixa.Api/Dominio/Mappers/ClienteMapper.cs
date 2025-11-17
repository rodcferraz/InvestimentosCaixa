using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    public class ClienteMapper : IClienteMapper
    {
        public Cliente ToEntity(ClienteDTOCadastroRequest clienteDto)
        {
            if (!Enum.IsDefined(typeof(PerfilRiscoClienteEnum), clienteDto.Liquidez))
            {
                throw new ConvertEnumException(typeof(PerfilRiscoClienteEnum), clienteDto.Liquidez);
            }

            return new Cliente
            {
                Nome = clienteDto.Nome,
                Email = clienteDto.Email.ToLower(),
                Liquidez = clienteDto.Liquidez,
            };
        }

        public Cliente ToBaseEntity(ClienteDTOBaseRequest clienteDto)
        {
            if (!Enum.IsDefined(typeof(PerfilRiscoClienteEnum), clienteDto.Liquidez))
            {
                throw new ConvertEnumException(typeof(PerfilRiscoClienteEnum), clienteDto.Liquidez);
            }

            return new Cliente
            {
                Nome = clienteDto.Nome,
                Email = clienteDto.Email,
                Liquidez = clienteDto.Liquidez
            };
        }

        public ClienteDTOResponse ToDtoResponse(Cliente cliente)
        {
            return new ClienteDTOResponse
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Liquidez = cliente.Liquidez
            };
        }

        public List<ClienteDTOResponse> ToDtoResponseList(IEnumerable<Cliente> clientes)
        {
            return clientes.Select(x => ToDtoResponse(x)).ToList();
        }

        public Cliente ToEntity(ClienteDTORequest clienteDto)
        {
            var cliente = ToBaseEntity(clienteDto);
            cliente.Id = clienteDto.Id;
            return cliente;
        }
    }
}
