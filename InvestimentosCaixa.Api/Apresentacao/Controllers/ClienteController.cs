using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Aplicacao.Servicos.Interfaces;
using InvestimentosCaixa.Api.Apresentacao.Atributos;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
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

        [Authorize]
        [HttpPost("cadastrar-cliente")]
        public async Task<ActionResult> CadastrarCliente(ClienteDTOCadastroRequest dto)
        {
            try
            {
                var clienteId = await _clienteServico.CadastrarClienteAsync(dto);
                _logger.LogInformation($"ClienteId {clienteId} cadastrado com sucesso.");
                return Ok($"ClienteId {clienteId} cadastrado.");
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
                return Ok("Cliente deletado.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao deletar cliente com Id {id}: {e.Message}");
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }

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

                await _clienteServico.AtualizarSenhaClienteAsync(
                    clienteSenhaDto.Email.ToLower(), 
                    clienteSenhaDto.SenhaAtual, 
                    clienteSenhaDto.NovaSenha);

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
            catch (Exception e)
            {
                _logger.LogError($"Erro ao gerar perfil de risco para o cliente com ID {clienteId}: {e.Message}");
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        } 
    }
}
