using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InvestimentosCaixa.Api.Apresentacao.Controllers
{
    /// <summary>
    /// Controller de Produtos
    /// </summary>
    [Route("v1/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoServico _produtoServico;

        public ProdutoController(IProdutoServico produtoServico)
        {
            _produtoServico = produtoServico;
        }

        /// <summary>
        /// Retornar todos os produtos cadastrados e ativos
        /// </summary>
        /// <returns>Lista de produtos</returns>
        [HttpGet]
        public async Task<ActionResult> ListarTodosProdutos()
        {
            try
            {
                var listarProdutos = await _produtoServico.ListarTodosProdutosAtivosAsync();

                return Ok(listarProdutos);
            }
            catch (Exception e)
            {
                return BadRequest($"Não foi possível listar os produtos: {e.Message}");
            }
        }

        /// <summary>
        /// Retornar produto por ID
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <returns>Produto cadastrado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> BuscarProdutoPorId(int id)
        {
            try
            {
                var produto = await _produtoServico.DetalhesProdutoAsync(id);

                if (produto == null)
                {
                    return NotFound($"Produto com {id} não encontrado");
                }

                return Ok(produto);
            }
            catch (Exception e)
            {
                return BadRequest($"Não foi possível listar produto: {e.Message}");
            }
        }

        /// <summary>
        /// Cadastra um novo produto
        /// </summary>
        /// <param name="produtoDto">Dto de produto para cadastro</param>
        [HttpPost]
        public async Task<ActionResult> CadastrarProduto([FromBody] ProdutoDTOBaseRequest produtoDto)
        {
            try
            {
                await _produtoServico.AdicionarProdutoAsync(produtoDto);
                return Ok("Produto cadastrado com sucesso!");
            }
            catch(ConvertEnumException e)
            {
                return BadRequest(e.Message);
            }
            catch(Exception e)
            {
                return BadRequest($"Não foi possível cadastrar produto: {e.Message}");
            }
        }

        /// <summary>
        /// Atualizar produto existente
        /// </summary>
        /// <param name="id">Id do produto a ser modificado</param>
        /// <param name="produto">ProdutoDTO com campo(s) atualizado(s)</param>
        /// <returns>ProdutoAtualizado</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> AtualizarProduto(int id, [FromBody] ProdutoDTORequest produto)
        {
            try
            {
                if (id != produto.Id)
                {
                    return BadRequest("O ID do produto não corresponde ao ID informado na URL.");
                }

                var produtoDb = await _produtoServico.ListarProdutoAtivoPorNomeAsync(produto.Nome);

                if (produtoDb != null)
                {
                    return BadRequest("Já existe um produto cadastrado com esse nome.");
                }

                var produtoAtualizado = await _produtoServico.AtualizarProdutoAsync(produto);

                if (produtoAtualizado == null)
                {
                    return NotFound($"Produto com ID {id} não encontrado para atualização.");
                }
                return Ok(produtoAtualizado);
            }
            catch (ConvertEnumException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest($"Não foi possível atualizar o produto: {e.Message}");
            }
        }

        /// <summary>
        /// Realizar deleção lógica do produto
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletarProduto(int id)
        {
            try
            {
                var resultado = await _produtoServico.RemoverProdutoAsync(id);
                if (!resultado)
                {
                    return NotFound($"Produto com ID {id} não encontrado para deleção.");
                }
                return Ok("Produto deletado com sucesso!");
            }
            catch (Exception e)
            {
                return BadRequest($"Não foi possível deletar o produto: {e.Message}");
            }
        }
    }
}
