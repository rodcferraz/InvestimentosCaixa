using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    /// <summary>
    /// Serviço responsável por gerenciar operações relacionadas a produtos.
    /// </summary>
    public interface IProdutoServico
    {
        /// <summary>
        /// Exibe detalhes do produto pelo Id
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <returns>Dados do produto</returns>
        Task<ProdutoDTOResponse?> DetalhesProdutoAsync(int id);

        /// <summary>
        /// Listar todos os produtos ativos
        /// </summary>
        /// <returns>Lista de produtos ativos</returns>
        Task<List<ProdutoDTOResponse>?> ListarTodosProdutosAtivosAsync();

        /// <summary>
        /// Adiciona produto informado
        /// </summary>
        /// <param name="produtoDto">Requisição para cadastro de produto</param>
        /// <returns>Id do produto gerado</returns>
        Task<int> AdicionarProdutoAsync(ProdutoDTOBaseRequest produtoDto);

        /// <summary>
        /// Atualiza produto informado
        /// </summary>
        /// <param name="produtoDto">Requisição para atualização de produto</param>
        /// <returns>Produto com campos atualizados</returns>
        /// <exception cref="ConvertEnumException">Lança exceção quando <see cref="RiscoProdutoEnum"/> 
        /// ou <see cref="TipoProdutoEnum"/> não foram informados corretamente</exception>
        Task<ProdutoDTOResponse?> AtualizarProdutoAsync(ProdutoDTORequest produtoDto);

        /// <summary>
        /// Remoção lógica do produto pelo Id
        /// </summary>
        /// <param name="idProduto">Id do produto</param>
        /// <returns>Confirmação de deleção lógica do produto</returns>
        Task<bool> RemoverProdutoAsync(int idProduto);

        /// <summary>
        /// Listar todos os produtos ativos pelo nome
        /// </summary>
        /// <param name="nomeProduto">Nome do produto</param>
        /// <returns>Lista de produtos ativos pelo nome</returns>
        Task<ProdutoDTOResponse?> ListarProdutoAtivoPorNomeAsync(string nomeProduto);

        /// <summary>
        /// Exibe produto ativo por tipo
        /// </summary>
        /// <param name="tipoProduto">Tipo do produto <see cref="TipoProdutoEnum"/></param>
        /// <returns>Produto encontrado</returns>
        /// <exception cref="ConvertEnumException">Lança exceção quando tipo informado não está mapeado em <see cref="TipoProdutoEnum"/></exception>
        Task<ProdutoDTOResponse?> ListarProdutoAtivoPorTipoAsync(string tipoProduto);

        /// <summary>
        /// Listar todos os produtos ativos por perfil de cliente
        /// </summary>
        /// <param name="idPerfil">Perfil de cliente</param>
        /// <returns>Lista de produtos ativos que corres</returns>
        Task<List<ProdutoDTOResponse>> ListarProdutosAtivosPorPerfilAsync(int idPerfil);

    }
}
