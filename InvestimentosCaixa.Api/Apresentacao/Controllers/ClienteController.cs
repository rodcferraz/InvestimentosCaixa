using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Aplicacao.Servicos.Interfaces;
using InvestimentosCaixa.Api.Apresentacao.Atributos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    /// <summary>
    /// Fornece endpoints para gerenciar operações relacionadas a clientes, incluindo listar, recuperar, criar, atualizar,
    /// e excluir clientes, bem como atualizar senhas e gerar perfis de risco de clientes.
    /// </summary>
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteServico _clienteServico;
        private readonly IGerarPerfilClienteServico _gerarPerfilClienteServico;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(
            IClienteServico clienteService,
            IGerarPerfilClienteServico gerarPerfilClienteServico,
            ILogger<ClienteController> logger)
        {
            _clienteServico = clienteService;
            _gerarPerfilClienteServico = gerarPerfilClienteServico;
            _logger = logger;
        }

        /// <summary>
        /// Recupera uma lista de todos os clientes ativos.
        /// </summary>
        /// <returns> Lista de todos os clientes ativos.</returns>
        /// <response code = "200"> Listagem de clientes ativos</response>
        /// <response code = "204"> Clientes ativos não encontrados </response>
        /// <response code = "500"> Erro interno no servidor </response>
        [Authorize]
        [HttpGet("listar-clientes")]
        public async Task<ActionResult> ListarTodosClientes()
        {
            try
            {
                var listarProdutos = await _clienteServico.ListarTodosClientesAtivosAsync();

                if (listarProdutos == null || listarProdutos.Count == 0)
                {
                    _logger.LogInformation("Nenhum cliente ativo encontrado.");
                    return NotFound("Nenhum cliente ativo encontrado.");
                }

                _logger.LogInformation("Listagem de clientes realizada com sucesso.");
                return Ok(listarProdutos);
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao listar clientes: {e.Message}");
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

        /// <summary>
        /// Recupera os detalhes de um cliente pelo seu identificador único.
        /// </summary>
        /// <param name="id">O identificador único do cliente a ser recuperado.</param>
        /// <returns>Cliente ativo</returns>
        /// <response code = "200"> Cliente ativo encontrado </response>
        /// <response code = "404"> Cliente ativo não encontrado </response>
        /// <response code = "500"> Erro interno no servidor </response>
        [Authorize]
        [HttpGet("buscar-cliente/{id}")]
        public async Task<ActionResult> BuscarClientePorId(int id)
        {
            try
            {
                var cliente = await _clienteServico.DetalhesClienteAsync(id);

                if (cliente == null)
                {
                    _logger.LogWarning($"Cliente com {id} não encontrado.");
                    return NotFound($"Cliente com {id} não encontrado");
                }
                _logger.LogInformation($"Cliente com {id} encontrado.");
                return Ok(cliente);
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao buscar cliente com Id {id}: {e.Message}");
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

        /// <summary>
        /// Registra um novo cliente no sistema.
        /// </summary>
        /// <param name="dto">Um objeto contendo os detalhes do cliente a ser registrado.</param>
        /// <returns>Id do cliente cadastrado</returns>
        /// <response code = "201"> Cliente cadastrado com sucesso </response>
        /// <response code = "400"> Requisição inválida </response>
        /// <response code = "500"> Erro interno no servidor </response>
        [HttpPost("cadastrar-cliente")]
        public async Task<ActionResult> CadastrarCliente(ClienteDTOCadastroRequest dto)
        {
            try
            {
                var idCliente = await _clienteServico.CadastrarClienteAsync(dto);
                _logger.LogInformation($"ClienteId {idCliente} cadastrado com sucesso.");
                return Created("cadastrar-cliente", new { idCliente });
            }
            catch (ConvertEnumException e)
            {
                _logger.LogError($"Erro ao converter enum durante o cadastro do cliente: {e.Message}");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao cadastrar cliente: {e.Message}");
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

        /// <summary>
        /// Atualiza os detalhes de um cliente existente.
        /// </summary>
        /// <param name="id">O identificador exclusivo do cliente a ser atualizado. Deve corresponder ao ID no corpo da solicitação.</param>
        /// <param name="cliente">Um objeto contendo os detalhes atualizados do cliente. A propriedade <see cref="ClienteDTORequest.Id"/> deve corresponder
        /// ao parâmetro <paramref name="id"/>.</param>
        /// <returns> Cliente atualizado </returns>
        /// <response code = "200"> Cliente atualizado com sucesso </response>
        /// <response code = "400"> Requisição inválida </response>
        /// <response code = "404"> Cliente ativo não encontrado </response>
        /// <response code = "500"> Erro interno no servidor </response>
        [Authorize]
        [HttpPut("atualizar-cliente/{id}")]
        public async Task<ActionResult> AtualizarCliente(int id, ClienteDTORequest cliente)
        {
            try
            {
                if (id != cliente.Id)
                {
                    _logger.LogWarning($"ID do cliente na URL ({id}) não corresponde ao ID no corpo da requisição ({cliente.Id}).");
                    return BadRequest("O ID do cliente não corresponde ao ID informado na URL.");
                }

                var clienteDb = await _clienteServico.ListarClienteAtivoPorEmailAsync(cliente.Nome);

                if (clienteDb != null)
                {
                    _logger.LogWarning($"Já existe um cliente cadastrado com o nome {cliente.Nome}.");
                    return BadRequest("Já existe um cliente cadastrado com esse nome.");
                }

                var clienteAtualizado = await _clienteServico.AtualizarClienteAsync(cliente);

                if (clienteAtualizado == null)
                {
                    _logger.LogWarning($"Cliente com ID {id} não encontrado para atualização.");
                    return NotFound($"Cliente com ID {id} não encontrado.");
                }

                _logger.LogInformation($"Cliente com ID {id} atualizado com sucesso.");
                return Ok(clienteAtualizado);
            }
            catch (ConvertEnumException e)
            {
                _logger.LogError($"Erro ao converter enum durante a atualização do cliente: {e.Message}");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao atualizar cliente com Id {cliente.Id}: {e.Message}");
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

        /// <summary>
        /// Exclui um cliente com o identificador especificado.
        /// </summary>
        /// <param name="id">O identificador exclusivo do cliente a ser excluído.</param>
        /// <returns> Retorna se o cliente foi excluído com sucesso </returns>
        /// <response code = "200"> Cliente deletado com sucesso </response>
        /// <response code = "404"> Cliente ativo não encontrado </response>
        /// <response code = "500"> Erro interno no servidor </response>
        [Authorize]
        [HttpDelete("remover-cliente/{id}")]
        public async Task<ActionResult> DeletarCliente(int id)
        {
            try
            {
                var resultado = await _clienteServico.RemoverClienteAsync(id);
                if (!resultado)
                {
                    _logger.LogWarning($"Cliente com ID {id} não encontrado para deleção.");
                    return NotFound($"Cliente com ID {id} não encontrado para deleção.");
                }
                _logger.LogInformation($"Cliente com ID {id} deletado.");
                return Ok(new { deletado = resultado});
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao deletar cliente com Id {id}: {e.Message}");
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

        /// <summary>
        /// Atualiza a senha de um cliente com base nas credenciais fornecidas.
        /// </summary>
        /// <param name="clienteSenhaDto">Um objeto contendo o e-mail do cliente, a senha atual, a nova senha e a confirmação da nova
        /// senha.</param>
        /// <returns> Confirmação de mudança de senha</returns>
        /// <response code = "200"> Senha do cliente atualizada com sucesso </response>
        /// <response code = "400"> Senha incorreta </response>
        /// <response code = "404"> Cliente ativo não encontrado </response>
        /// <response code = "500"> Erro interno no servidor </response>
        [Authorize]
        [HttpPost("atualizar-senha")]
        public async Task<ActionResult> AtualizarSenhaCliente(AtualizarSenhaClienteDTORequest clienteSenhaDto)
        {
            try
            {
                if (!clienteSenhaDto.NovaSenha.Equals(clienteSenhaDto.ConfirmarNovaSenha))
                {
                    _logger.LogWarning("Campos de nova senha não são idênticos.");
                    return BadRequest("Campos de nova senha não são idênticos");
                }

                var senhaAtualizada = await _clienteServico.AtualizarSenhaClienteAsync(
                                        clienteSenhaDto.Email.ToLower(), 
                                        clienteSenhaDto.SenhaAtual, 
                                        clienteSenhaDto.NovaSenha);

                if (!senhaAtualizada)
                {
                    return NotFound("Cliente não encontrado para atualização de senha.");
                }

                _logger.LogInformation($"Senha do cliente com email {clienteSenhaDto.Email} atualizada com sucesso.");
                return Ok("Senha atualizada com sucesso");
            }
            catch (SenhaIncorretaException erro)
            {
                _logger.LogWarning($"Senha incorreta para o cliente com email {clienteSenhaDto.Email}: {erro.Message}");
                return BadRequest("Senha incorreta");
            }
            catch(Exception erro)
            {
                _logger.LogError($"Erro ao atualizar a senha do cliente com email {clienteSenhaDto.Email}: {erro.Message}");
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

        /// <summary>
        /// Recupera o perfil de risco de um cliente com base em seu identificador único.
        ///</summary>
        /// <param name="clienteId">O identificador único do cliente cujo perfil de risco deve ser recuperado.</param>
        /// <returns>Perfil de risco do cliente</returns>
        /// <response code = "200"> Perfil de risco do cliente atualizado </response>
        /// <response code = "400"> Requisição inválida </response>
        /// <response code = "404"> Cliente ativo não encontrado </response>
        /// <response code = "500"> Erro interno no servidor </response>
        [Authorize]
        [Telemetria]
        [HttpGet("perfil-risco/{clienteId}")]
        public async Task<ActionResult> ExibirPerfilRiscoCliente(int clienteId)
        {
            try
            {
                var perfilRisco = await _gerarPerfilClienteServico.GerarPerfilCiente(clienteId);

                _logger.LogInformation($"Perfil de risco {perfilRisco.Perfil} atribuído para o cliente com ID {clienteId}.");

                return Ok(perfilRisco);
            }
            catch(ConvertEnumException error)
            {
                _logger.LogError($"Erro ao converter enum ao gerar perfil de risco para o cliente com ID {clienteId}: {error.Message}");
                return BadRequest(error.Message);
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogWarning($"Cliente com ID {clienteId} não encontrado ao gerar perfil de risco: {e.Message}");
                return NotFound($"Cliente com ID {clienteId} não encontrado.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao gerar perfil de risco para o cliente com ID {clienteId}: {e.Message}");
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        } 
    }
}
