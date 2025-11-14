using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;

namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    public interface IProdutoServico
    {
        Task<ProdutoDTOResponse?> DetalhesProduto(int id);
        Task<List<ProdutoDTOResponse>?> ListarTodosProdutosAtivos();
        Task AdicionarProduto(ProdutoDTOBaseRequest produtoDto);
        Task<ProdutoDTOResponse?> AtualizarProduto(ProdutoDTORequest produtoDto);
        Task<bool> RemoverProduto(int idAluno);
        Task<ProdutoDTOResponse?> ListarProdutoAtivoPorNome(string nomeProduto);
    }
}
