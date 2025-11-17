using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Mappers.Interfaces
{
    public interface IClienteMapper
    {
        ClienteDTOResponse ToDtoResponse(Cliente cliente);
        Cliente ToEntity(ClienteDTOCadastroRequest clienteDto);
        Cliente ToBaseEntity(ClienteDTOBaseRequest clienteDto);
        Cliente ToEntity(ClienteDTORequest clienteDto);
        List<ClienteDTOResponse> ToDtoResponseList(IEnumerable<Cliente> clientes);
    }
}
