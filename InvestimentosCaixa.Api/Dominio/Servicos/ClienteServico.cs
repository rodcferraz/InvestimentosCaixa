using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    public class ClienteServico : IClienteServico
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IClienteMapper _clienteMapper;
        public ClienteServico(IClienteRepositorio clienteRepositorio, IClienteMapper clienteMapper) 
        {
            _clienteRepositorio = clienteRepositorio;
            _clienteMapper = clienteMapper;
        }

        public async Task<ClienteDTOResponse?> AtualizarClienteAsync(ClienteDTORequest clienteDto)
        {
            var clienteDb = await _clienteRepositorio.ListarPorIdAsync(clienteDto.Id);

            if (clienteDb == null)
                return null;

            clienteDb.Nome = clienteDto.Nome;
            clienteDb.Liquidez = clienteDto.Liquidez;

            var clienteAtualizado = await _clienteRepositorio.AtualizarAsync(clienteDb);

            return _clienteMapper.ToDtoResponse(clienteAtualizado);
        }

        public async Task<bool> AtualizarSenhaClienteAsync(int idCliente, string senhaAtual, string novaSenha)
        {
            var clienteDb = await _clienteRepositorio.ListarPorIdAsync(idCliente);

            if (clienteDb == null || clienteDb.Ativo == false)
                return false;

            if (clienteDb.SenhaHash != senhaAtual)
                throw new SenhaIncorretaException("Senha informada está incorreta");

            await _clienteRepositorio.AtualizarSenhaClienteAsync(idCliente, novaSenha);

            return true;

        }
        public async Task CadastrarClienteAsync(ClienteDTOBaseRequest dto)
        {
            var clienteDb = _clienteMapper.ToBaseEntity(dto);
            await _clienteRepositorio.AdicionarAsync(clienteDb);
        }

        public async Task<ClienteDTOResponse> DetalhesClienteAsync(int id)
        {
            var cliente = await _clienteRepositorio.ListarPorIdAsync(id);
            if (cliente == null || cliente.Ativo == false)
                return null;
            return _clienteMapper.ToDtoResponse(cliente);
        }

        public async Task<ClienteDTOResponse?> ListarClienteAtivoPorEmailAsync(string nomeCliente)
        {
            var clienteDb = await _clienteRepositorio.ListarClienteAtivoPorEmailAsync(nomeCliente);
            if (clienteDb == null || clienteDb.Ativo == false)
                return null;
            return _clienteMapper.ToDtoResponse(clienteDb);
        }

        public async Task<List<ClienteDTOResponse>?> ListarTodosClientesAtivosAsync()
        {
            var clientes = await _clienteRepositorio.ListarTodosAsync();
            var clientesAtivos = clientes?.Where(x => x.Ativo).ToList();

            return (clientesAtivos != null && clientesAtivos.Count != 0) ?
                _clienteMapper.ToDtoResponseList(clientesAtivos) :
                null;
        }

        public async Task<bool> RemoverClienteAsync(int id)
        {
            var produtoDb = await _clienteRepositorio.ListarPorIdAsync(id);
            if (produtoDb == null)
                return false;

            produtoDb.Ativo = false;

            _ = await _clienteRepositorio.AtualizarAsync(produtoDb);

            return true;
        }
    }
}
