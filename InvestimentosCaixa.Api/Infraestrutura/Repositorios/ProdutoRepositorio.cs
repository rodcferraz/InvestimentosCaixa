using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Api.Infraestrutura.Repositorios
{
    public class ProdutoRepositorio : GenericoRepositorio<Produto>, IProdutoRepositorio
    {
        private readonly InvestimentosCaixaDbContext _context;

        public ProdutoRepositorio(InvestimentosCaixaDbContext context) : 
            base(context)
        {
            _context = context;
        }

        public async Task<Produto?> ListarProdutoPorNome(string nomeProduto)
        {
            return await _context.Produtos
                        .FirstOrDefaultAsync(p =>
                            p.Nome == nomeProduto);

        }

        public async Task<Produto?> ListarProdutoPorTipo(int tipoProduto)
        {
            return await _context.Produtos
                        .FirstOrDefaultAsync(p =>
                            p.Tipo == tipoProduto);
        }
    }
}
