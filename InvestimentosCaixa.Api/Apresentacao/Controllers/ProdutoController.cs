using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;
using InvestimentosCaixa.Api.Apresentacao.Atributos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    /// <summary>
    /// Fornece endpoints para gerenciar produtos, incluindo a criação, atualização, deleção lógica e recuperação de produtos.
    /// </summary>
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoServico _produtoServico;
        private readonly ILogger<ProdutoController> _logger;    

        public ProdutoController(IProdutoServico produtoServico, ILogger<ProdutoController> logger)
        {
            _produtoServico = produtoServico;
            _logger = logger;
        }

        /// <summary>
        /// Retornar todos os produtos cadastrados e ativos
        /// </summary>
        /// <returns>Lista de produtos ativos</returns>
        [Authorize]
        [HttpGet("listar-produtos")]
        public async Task<ActionResult> ListarTodosProdutos()
        {
            try
            {
                var listarProdutos = await _produtoServico.ListarTodosProdutosAtivosAsync();

                if (listarProdutos == null || listarProdutos.Count == 0)
                {
                    _logger.LogInformation("Nenhum produto ativo encontrado.");
                    return NoContent();
                }

                _logger.LogInformation("Listagem de produtos realizada com sucesso.");

                return Ok(listarProdutos);
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao listar produtos: {e.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        /// <summary>
        /// Retornar produto por ID
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <returns>Produto cadastrado</returns>
        [Authorize]
        [HttpGet("buscar-produto/{id}")]
        public async Task<ActionResult> BuscarProdutoPorId(int id)
        {
            try
            {
                var produto = await _produtoServico.DetalhesProdutoAsync(id);

                if (produto == null)
                {
                    _logger.LogWarning($"Produto com {id} não encontrado.");
                    return NotFound($"Produto com {id} não encontrado");
                }

                _logger.LogInformation($"Produto {produto} encontrado.");
                return Ok(produto);
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao buscar produto: {e.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        /// <summary>
        /// Cadastra um novo produto
        /// </summary>
        /// <param name="produtoDto">Dto de produto para cadastro</param>
        [Authorize]
        [HttpPost("cadastrar-produto")]
        public async Task<ActionResult> CadastrarProduto([FromBody] ProdutoDTOBaseRequest produtoDto)
        {
            try
            {
                var produtoDb = await _produtoServico.ListarProdutoAtivoPorNomeAsync(produtoDto.Nome);

                if (produtoDb != null)
                {
                    _logger.LogWarning($"Já existe um produto {produtoDto.Nome} cadastrado com esse nome.");
                    return BadRequest("Já existe um produto cadastrado com esse nome.");
                }
                var idProduto = await _produtoServico.AdicionarProdutoAsync(produtoDto);
                return Created("cadastrar-produto", new { idProduto });
            }
            catch(ConvertEnumException e)
            {
                _logger.LogError($"Erro ao converter enum durante a atualização do produto {produtoDto.Nome}: {e.Message}");
                return BadRequest(e.Message);
            }
            catch(Exception e)
            {
                _logger.LogError($"Erro ao cadastrar produto: {e.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        /// <summary>
        /// Atualizar produto existente
        /// </summary>
        /// <param name="id">Id do produto a ser modificado</param>
        /// <param name="produto">ProdutoDTO com campo(s) atualizado(s)</param>
        /// <returns>ProdutoAtualizado</returns>
        [Authorize]
        [HttpPut("atualizar-produto/{id}")]
        public async Task<ActionResult> AtualizarProduto(int id, [FromBody] ProdutoDTORequest produto)
        {
            try
            {
                if (id != produto.Id)
                {
                    _logger.LogWarning($"O ID {produto.Id} do produto não corresponde ao ID {id} informado na URL.");
                    return BadRequest("O ID do produto não corresponde ao ID informado na URL.");
                }

                var produtoDb = await _produtoServico.ListarProdutoAtivoPorNomeAsync(produto.Nome);

                if (produtoDb != null)
                {
                    _logger.LogWarning($"Já existe um produto {produto.Nome} cadastrado com esse nome.");
                    return BadRequest("Já existe um produto cadastrado com esse nome.");
                }

                var produtoAtualizado = await _produtoServico.AtualizarProdutoAsync(produto);

                if (produtoAtualizado == null)
                {
                    _logger.LogWarning($"Produto com ID {id} não encontrado para atualização.");
                    return NotFound($"Produto com ID {id} não encontrado para atualização.");
                }

                _logger.LogInformation($"Produto {produto} atualizado com sucesso.");
                return Ok(produtoAtualizado);
            }
            catch (ConvertEnumException e)
            {
                _logger.LogError($"Erro ao converter enum durante a atualização do produto {produto.Id}: {e.Message}");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError($"Não foi possível atualizar o produto: {e.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        /// <summary>
        /// Realizar deleção lógica do produto
        /// </summary>
        /// <param name="id"></param>
        [Authorize]
        [HttpDelete("remover-produto/{id}")]
        public async Task<ActionResult> DeletarProduto(int id)
        {
            try
            {
                var resultado = await _produtoServico.RemoverProdutoAsync(id);
                if (!resultado)
                {
                    _logger.LogWarning($"Produto com ID {id} não encontrado para deleção.");
                    return NotFound($"Produto com ID {id} não encontrado para deleção.");
                }

                _logger.LogInformation($"Produto com ID {id} deletado com sucesso.");
                return Ok(new { deletado = resultado });
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao deletar produto com Id {id}: {e.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }

        /// <summary>
        /// Recupera uma lista de produtos recomendados para o perfil especificado.
        /// </summary>
        /// <returns>Lista de produtos recomendados para o perfil especificado.</returns>
        /// <response code = "200"> Listagem de produtos recomendados </response>
        /// <response code = "204"> Nenhuma listagem encontrada </response>
        /// <response code = "500"> Erro interno no servidor </response>
        [Telemetria]
        [Authorize]
        [HttpGet("produtos-recomendados/{perfil}")]
        public async Task<ActionResult> ListarProdutosRecomendadosPorPerfil(string perfil)
        {
            try{
                if (!Enum.IsDefined(typeof(PerfilRiscoClienteEnum), perfil))
                {
                    throw new ConvertEnumException(typeof(PerfilRiscoClienteEnum), perfil);
                }

                int riscoProduto = (int)Enum.Parse(typeof(PerfilRiscoClienteEnum), perfil, ignoreCase: true);

                var produtosRecomendados = await _produtoServico.ListarProdutosAtivosPorPerfilAsync(riscoProduto);

                if (!produtosRecomendados.Any())
                {
                    _logger.LogInformation($"Nenhum produto recomendado encontrado para o perfil {perfil}.");
                    return NoContent();
                }
                
                _logger.LogInformation($"Produtos recomendados para o perfil {perfil} listados com sucesso.");
                return Ok(produtosRecomendados);

            }
            catch (ConvertEnumException error)
            {
                _logger.LogError($"Erro ao converter risco de produto para enum: {error.Message}");
                return BadRequest(error.Message);
            }
            catch(Exception erro)
            {
                _logger.LogError($"Erro ao listar produtos recomendados para o perfil {perfil}: {erro.Message}");
                return StatusCode(500, "Erro interno no servidor.");
            }
        }
    }
}
