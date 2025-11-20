using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;

namespace InvestimentosCaixa.Api.Dominio.Mappers.Interfaces
{
    /// <summary>
    /// Mapper para conversões da classe <see cref = "ClienteMapper"/>.
    /// </summary>
    public interface IClienteMapper
    {
        /// <summary>
        /// Realiza a conversão de <see cref="Cliente"/> para <see cref="ClienteDTOResponse"/>.
        /// </summary>
        /// <param name="cliente">Enttidade cliente</param>
        /// <returns>Retornar <see cref="ClienteDTOResponse"/> convertido</returns>
        ClienteDTOResponse ToDtoResponse(Cliente cliente);

        /// <summary>
        /// Realiza a conversão de requisição <see cref="ClienteDTOCadastroRequest"/> para a entidade <see cref="Cliente"/>.
        /// </summary>
        /// <param name="clienteDto">Requisição de cliente dto</param>
        /// <returns>Retornar cliente convertido</returns>
        /// <exception cref="ConvertEnumException">Exceção lançada quando perfil não existe no <see cref="PerfilRiscoClienteEnum"/></exception>
        Cliente ToEntity(ClienteDTOCadastroRequest clienteDto);

        /// <summary>
        /// Realiza a conversão de requisição <see cref="ClienteDTOBaseRequest"/> para a entidade <see cref="Cliente"/>.
        /// </summary>
        /// <param name="clienteDto">Requisição de cliente dto</param>
        /// <returns>Retornar cliente convertido</returns>
        /// <exception cref="ConvertEnumException">Exceção lançada quando perfil não existe no <see cref="PerfilRiscoClienteEnum"/></exception>
        Cliente ToBaseEntity(ClienteDTOBaseRequest clienteDto);

        /// <summary>
        /// Realiza a conversão de  <see cref="ClienteDTORequest"/> para a entidade <see cref="Cliente"/>.
        /// </summary>
        /// <param name="clienteDto">Requisição de cliente Dto <see cref="ClienteDTORequest"/></param>
        /// <returns>Retornar <see cref="Cliente"/> convertido</returns>
        Cliente ToEntity(ClienteDTORequest clienteDto);

        /// <summary>
        /// Realiza a conversão de  <see cref="IEnumerable{Cliente}"/> para  <see cref="List{ClienteDTOResponse}"/>.
        /// </summary>
        /// <param name="clientes">Lista de entidade de clientes</param>
        /// <returns>Retornar <see cref="List{ClienteDTOResponse}"/> convertido</returns>
        List<ClienteDTOResponse> ToDtoResponseList(IEnumerable<Cliente> clientes);
    }
}
