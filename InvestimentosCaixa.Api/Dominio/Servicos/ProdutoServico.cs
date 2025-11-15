using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    public class ProdutoServico : IProdutoServico
    {
        private readonly IProdutoMapper _produtoMapper;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutoServico(IProdutoMapper produtoMapper,
                              IProdutoRepositorio produtoRepositorio)
        {
            _produtoMapper = produtoMapper;
            _produtoRepositorio = produtoRepositorio;
        }

        public async Task AdicionarProdutoAsync(ProdutoDTOBaseRequest produtoDto)
        {
            var produtoDb = _produtoMapper.ToBaseEntity(produtoDto);

            await _produtoRepositorio.AdicionarAsync(produtoDb);
        }

        public async Task<ProdutoDTOResponse?> AtualizarProdutoAsync(ProdutoDTORequest produtoDto)
        {
            var produtoDb = await _produtoRepositorio.ListarPorIdAsync(produtoDto.Id);

            if (produtoDb == null)
                return null;

            if (!Enum.TryParse(produtoDto.Risco, out RiscoProduto riscoProduto))
            {
                throw new ConvertEnumException(typeof(RiscoProduto), produtoDto.Risco);
            }

            if (!Enum.TryParse(produtoDto.Tipo, out TipoProduto tipoProduto))
            {
                throw new ConvertEnumException(typeof(TipoProduto), produtoDto.Tipo);
            }

            produtoDb.Nome = produtoDto.Nome;
            produtoDb.Rentabilidade = produtoDto.Rentabilidade;
            produtoDb.Risco = (int) riscoProduto;
            produtoDb.Tipo =(int) tipoProduto;

            var produtoAtualizado = await _produtoRepositorio.AtualizarAsync(produtoDb);

            return _produtoMapper.ToDtoResponse(produtoAtualizado);
        }

        public async Task<ProdutoDTOResponse?> DetalhesProdutoAsync(int id)
        {
            var produto = await _produtoRepositorio.ListarPorIdAsync(id);
            if (produto == null)
                return null;
            return _produtoMapper.ToDtoResponse(produto);
        }

        public async Task<ProdutoDTOResponse?> ListarProdutoAtivoPorNomeAsync(string nomeProduto)
        {
            var produtoDb = await _produtoRepositorio.ListarProdutoPorNome(nomeProduto);
            if (produtoDb == null || produtoDb.Ativo == false)
                return null;
            return _produtoMapper.ToDtoResponse(produtoDb);
        }

        public async Task<ProdutoDTOResponse?> ListarProdutoAtivoPorTipoAsync(string tipoProduto)
        {
            if (!Enum.TryParse(tipoProduto, out TipoProduto TipoProduto))
            {
                throw new ConvertEnumException(typeof(TipoProduto), tipoProduto);
            }

            var produtoDb = await _produtoRepositorio.ListarProdutoPorTipo((int)TipoProduto);

            if (produtoDb == null)
            {
                return null;
            }

            return _produtoMapper.ToDtoResponse(produtoDb);
        }

        public async Task<List<ProdutoDTOResponse>?> ListarTodosProdutosAtivosAsync()
        {
            var produtos = await _produtoRepositorio.ListarTodosAsync();
            var produtosAtivos = produtos?.Where(x => x.Ativo).ToList();

            return (produtosAtivos != null && produtosAtivos.Count != 0) ?
                _produtoMapper.ToDtoResponseList(produtosAtivos) :
                null;
        }

        public async Task<bool> RemoverProdutoAsync(int idAluno)
        {
            var produtoDb = await _produtoRepositorio.ListarPorIdAsync(idAluno);
            if (produtoDb == null)
                return false;

            produtoDb.Ativo = false;

            _ = await _produtoRepositorio.AtualizarAsync(produtoDb);

            return true;
        }
    }
}
