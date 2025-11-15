using InvestimentosCaixa.Api.Dominio.Entidades;

namespace InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces
{
    public interface IProdutoRepositorio : IGenericoRepositorio<Produto>
    {
        Task<Produto?> ListarProdutoPorNome(string nomeProduto);
        Task<Produto?> ListarProdutoPorTipo(int tipoProduto);
    }
}
