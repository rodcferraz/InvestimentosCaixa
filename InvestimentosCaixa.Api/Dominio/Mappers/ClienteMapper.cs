using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    /// <summary>
    /// Mapper para conversões da classe <see cref = "ClienteMapper"/>.
    /// </summary>
    public class ClienteMapper : IClienteMapper
    {
        /// <summary>
        /// Realiza a conversão de requisição <see cref="ClienteDTOCadastroRequest"/> para a entidade <see cref="Cliente"/>.
        /// </summary>
        /// <param name="clienteDto">Requisição de cliente dto</param>
        /// <returns>Retornar cliente convertido</returns>
        /// <exception cref="ConvertEnumException">Exceção lançada quando perfil não existe no <see cref="PerfilRiscoClienteEnum"/></exception>
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

        /// <summary>
        /// Realiza a conversão de requisição <see cref="ClienteDTOBaseRequest"/> para a entidade <see cref="Cliente"/>.
        /// </summary>
        /// <param name="clienteDto">Requisição de cliente dto</param>
        /// <returns>Retornar cliente convertido</returns>
        /// <exception cref="ConvertEnumException">Exceção lançada quando perfil não existe no <see cref="PerfilRiscoClienteEnum"/></exception>
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

        /// <summary>
        /// Realiza a conversão de <see cref="Cliente"/> para  <see cref="ClienteDTOResponse"/>.
        /// </summary>
        /// <param name="cliente">Enttidade cliente</param>
        /// <returns>Retornar <see cref="ClienteDTOResponse"/> convertido</returns>
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

        /// <summary>
        /// Realiza a conversão de  <see cref="IEnumerable{Cliente}"/> para  <see cref="List{ClienteDTOResponse}"/>.
        /// </summary>
        /// <param name="clientes">Lista de entidade de clientes</param>
        /// <returns>Retornar <see cref="List{ClienteDTOResponse}"/> convertido</returns>
        public List<ClienteDTOResponse> ToDtoResponseList(IEnumerable<Cliente> clientes)
        {
            return clientes.Select(x => ToDtoResponse(x)).ToList();
        }

        /// <summary>
        /// Realiza a conversão de  <see cref="ClienteDTORequest"/> para a entidade <see cref="Cliente"/>.
        /// </summary>
        /// <param name="clienteDto">Requisição de cliente Dto <see cref="ClienteDTORequest"/></param>
        /// <returns>Retornar <see cref="Cliente"/> convertido</returns>
        public Cliente ToEntity(ClienteDTORequest clienteDto)
        {
            var cliente = ToBaseEntity(clienteDto);
            cliente.Id = clienteDto.Id;
            return cliente;
        }
    }
}
