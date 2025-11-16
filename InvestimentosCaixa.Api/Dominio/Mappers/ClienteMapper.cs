using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    public class ClienteMapper : IClienteMapper
    {
        public Cliente ToBaseEntity(ClienteDTOBaseRequest clienteDto)
        {
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
