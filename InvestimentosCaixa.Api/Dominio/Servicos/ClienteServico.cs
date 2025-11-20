using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    /// <summary>
    /// Classe de serviço responsável pelas operações relacionadas aos clientes.
    /// </summary>
    public class ClienteServico : IClienteServico
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IClienteMapper _clienteMapper;
        private readonly ILogger<ClienteServico> _logger;
        private readonly ISegurancaServico _segurancaServico;

        public ClienteServico(
            IClienteRepositorio clienteRepositorio, 
            IClienteMapper clienteMapper,
            ILogger<ClienteServico> logger,
            ISegurancaServico segurancaServico) 
        {
            _clienteRepositorio = clienteRepositorio;
            _clienteMapper = clienteMapper;
            _logger = logger;
            _segurancaServico = segurancaServico;
        }

        /// <summary>
        /// Atualiza os dados de um cliente existente.
        /// </summary>
        /// <param name="clienteDto">Dto do cliente</param>
        /// <returns>Cliente atualizado</returns>
        /// <exception cref="ConvertEnumException">Lança exceção caso o <see cref="PerfilRiscoClienteEnum"/>
        /// não esteja definido internamente</exception>
        public async Task<ClienteDTOResponse?> AtualizarClienteAsync(ClienteDTORequest clienteDto)
        {
            if (!Enum.IsDefined(typeof(PerfilRiscoClienteEnum), clienteDto.Liquidez))
            {
                throw new ConvertEnumException(typeof(PerfilRiscoClienteEnum), clienteDto.Liquidez);
            }

            var clienteDb = await _clienteRepositorio.ListarPorIdAsync(clienteDto.Id);

            if (clienteDb == null)
            {
                _logger.LogWarning($"Cliente com Id {clienteDto.Id} não encontrado para atualização.");
                return null;
            }

            clienteDb.Nome = clienteDto.Nome;
            clienteDb.Email = clienteDto.Email;
            clienteDb.Liquidez = clienteDto.Liquidez;

            var clienteAtualizado = await _clienteRepositorio.AtualizarAsync(clienteDb);

            _logger.LogInformation($"Cliente com Id {clienteDto.Id} atualizado com sucesso.");

            return _clienteMapper.ToDtoResponse(clienteAtualizado);
        }

        /// <summary>
        /// Atualiza a senha do cliente
        /// </summary>
        /// <param name="email">Email do cliente</param>
        /// <param name="senhaAtual">Senha atual do cliente</param>
        /// <param name="novaSenha">Nova senha do cliente</param>
        /// <returns>Confirmação de atualização de senha</returns>
        /// <exception cref="SenhaIncorretaException"></exception>
        public async Task<bool> AtualizarSenhaClienteAsync(string email, string senhaAtual, string novaSenha)
        {
            var clienteDb = await _clienteRepositorio.ListarClienteAtivoPorEmailAsync(email.ToLower());

            if (clienteDb == null || clienteDb.Ativo == false)
            {
                _logger.LogWarning($"Cliente com email {email} não encontrado para atualização de senha.");
                return false;
            }

            var senhaAtualCriptografada = _segurancaServico.CriptografarPasswordHash(senhaAtual);

            if (clienteDb.SenhaHash != senhaAtualCriptografada)
                throw new SenhaIncorretaException("Senha informada está incorreta");

            var novaSenhaCriptografada = _segurancaServico.CriptografarPasswordHash(novaSenha);

            _logger.LogInformation("Criptografando nova senha;");

            await _clienteRepositorio.AtualizarSenhaClienteAsync(email, novaSenhaCriptografada);

            _logger.LogInformation($"Senha do cliente com email {email} atualizada com sucesso.");

            return true;

        }

        /// <summary>
        /// Realiza o cadastro de um novo cliente.
        /// </summary>
        /// <param name="dto">Dados de requisição para a crição de um novo cliente</param>
        /// <returns>Retorna Id do cliente</returns>
        public async Task<int> CadastrarClienteAsync(ClienteDTOCadastroRequest dto)
        {
            var clienteDb = _clienteMapper.ToEntity(dto);
            clienteDb.SenhaHash = _segurancaServico.CriptografarPasswordHash(dto.Senha);

            _logger.LogInformation($"Cadastrando novo cliente com email: {dto.Email}");

            var cliente = await _clienteRepositorio.AdicionarAsync(clienteDb);
            _logger.LogInformation($"Cliente cadastrado com sucesso com Id: {cliente.Id}");

            return cliente.Id;
        }

        /// <summary>
        /// Detalhar um cliente pelo Id.
        /// </summary>
        /// <param name="id">Id do cliente</param>
        /// <returns>Dados do cliente</returns>
        public async Task<ClienteDTOResponse> DetalhesClienteAsync(int id)
        {
            var cliente = await _clienteRepositorio.ListarPorIdAsync(id);

            _logger.LogInformation($"Detalhes do cliente com Id {id} recuperados com sucesso.");

            if (cliente == null || cliente.Ativo == false)
            {
                _logger.LogWarning($"Cliente com Id {id} não encontrado ou inativo.");
                return null;
            }
            
            _logger.LogInformation($"Cliente com Id {id} encontrado.");

            return _clienteMapper.ToDtoResponse(cliente);
        }

        /// <summary>
        /// Lista clientes ativos por email
        /// </summary>
        /// <param name="email">Email do cliente</param>
        /// <returns>Retorna dados do cliente</returns>
        public async Task<ClienteDTOResponse?> ListarClienteAtivoPorEmailAsync(string email)
        {
            var clienteDb = await _clienteRepositorio.ListarClienteAtivoPorEmailAsync(email.ToLower());
            if (clienteDb == null || clienteDb.Ativo == false)
            {
                _logger.LogWarning($"Cliente com email {email} não encontrado ou inativo.");
                return null;
            }
            
            _logger.LogInformation($"Cliente com email {email} encontrado.");
            return _clienteMapper.ToDtoResponse(clienteDb);
        }

        /// <summary>
        /// Listar todos os clientes ativos
        /// </summary>
        /// <returns>Lista de todos os clientes ativos</returns>
        public async Task<List<ClienteDTOResponse>?> ListarTodosClientesAtivosAsync()
        {
            var clientes = await _clienteRepositorio.ListarTodosAsync();
            var clientesAtivos = clientes?.Where(x => x.Ativo).ToList();

            _logger.LogInformation("Listagem de todos os clientes ativos realizada com sucesso.");

            return (clientesAtivos != null && clientesAtivos.Count != 0) ?
                _clienteMapper.ToDtoResponseList(clientesAtivos) :
                null;
        }

        /// <summary>
        /// Remove logicamente um cliente pelo Id.
        /// </summary>
        /// <param name="id">Id do cliente</param>
        /// <returns>Retorna confirmação da deleção lógica do cliente</returns>
        public async Task<bool> RemoverClienteAsync(int id)
        {
            var produtoDb = await _clienteRepositorio.ListarPorIdAsync(id);

            if (produtoDb == null)
            {
                _logger.LogWarning($"Cliente com Id {id} não encontrado para remoção.");
                return false;
            }

            produtoDb.Ativo = false;

            _ = await _clienteRepositorio.AtualizarAsync(produtoDb);

            _logger.LogInformation($"Cliente com Id {id} inativado.");

            return true;
        }
    }
}
