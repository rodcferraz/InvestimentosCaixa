using InvestimentosCaixa.Api.Aplicacao.DTOs.Clientes;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteServico _clienteServico;

        public ClienteController(IClienteServico clienteService)
        {
            _clienteServico = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult> ListarTodosClientes()
        {
            try
            {
                var listarProdutos = await _clienteServico.ListarTodosClientesAtivosAsync();

                return Ok(listarProdutos);
            }
            catch (Exception e)
            {
                return BadRequest($"Não foi possível listar os clientes: {e.Message}");
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult> BuscarClientePorId(int id)
        {
            try
            {
                var cliente = await _clienteServico.DetalhesClienteAsync(id);

                if (cliente == null)
                {
                    return NotFound($"Cliente com {id} não encontrado");
                }

                return Ok(cliente);
            }
            catch (Exception e)
            {
                return BadRequest($"Não foi possível buscar cliente: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CadastrarCliente(ClienteDTOBaseRequest dto)
        {
            try
            {
                await _clienteServico.CadastrarClienteAsync(dto);
                return Ok($"Cliente {dto.Nome} cadastrado com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> AtualizarCliente(int id, ClienteDTORequest cliente)
        {
            try
            {
                if (id != cliente.Id)
                {
                    return BadRequest("O ID do cliente não corresponde ao ID informado na URL.");
                }

                var clienteDb = await _clienteServico.ListarClienteAtivoPorEmailAsync(cliente.Nome);

                if (clienteDb != null)
                {
                    return BadRequest("Já existe um cliente cadastrado com esse nome.");
                }

                var clienteAtualizado = await _clienteServico.AtualizarClienteAsync(cliente);

                if (clienteAtualizado == null)
                {
                    return NotFound($"Cliente com ID {id} não encontrado.");
                }
                return Ok(clienteAtualizado);
            }
            catch (ConvertEnumException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest($"Não foi possível atualizar o cliente: {e.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletarCliente(int id)
        {
            try
            {
                var resultado = await _clienteServico.RemoverClienteAsync(id);
                if (!resultado)
                {
                    return NotFound($"Cliente com ID {id} não encontrado para deleção.");
                }
                return Ok("Cliente deletado com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest($"Não foi possível deletar o cliente: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AtualizarSenhaCliente(AtualizarSenhaClienteDTORequest clienteSenhaDto)
        {
            try
            {
                if (!clienteSenhaDto.NovaSenha.Equals(clienteSenhaDto.ConfirmarNovaSenha))
                {
                    return BadRequest("Campos de nova senha não são idênticos");
                }

                await _clienteServico.AtualizarSenhaClienteAsync(
                    clienteSenhaDto.Id, 
                    clienteSenhaDto.SenhaAtual, 
                    clienteSenhaDto.NovaSenha);

                return Ok("Senha atuualizada com sucesso");
            }
            catch (SenhaIncorretaException erro)
            {
                return BadRequest(erro.Message);
            }
            catch(Exception erro)
            {
                return BadRequest(erro.Message);
            }
            
        }
    }
}
