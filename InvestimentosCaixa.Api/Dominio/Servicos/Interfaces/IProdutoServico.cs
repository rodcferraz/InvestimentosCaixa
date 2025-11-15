using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    public interface IProdutoServico
    {
        Task<ProdutoDTOResponse?> DetalhesProdutoAsync(int id);
        Task<List<ProdutoDTOResponse>?> ListarTodosProdutosAtivosAsync();
        Task AdicionarProdutoAsync(ProdutoDTOBaseRequest produtoDto);
        Task<ProdutoDTOResponse?> AtualizarProdutoAsync(ProdutoDTORequest produtoDto);
        Task<bool> RemoverProdutoAsync(int idAluno);
        Task<ProdutoDTOResponse?> ListarProdutoAtivoPorNomeAsync(string nomeProduto);
    }
}
