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

        public async Task AdicionarProduto(ProdutoDTOBaseRequest produtoDto)
        {
            var produtoDb = _produtoMapper.ToBaseEntity(produtoDto);

            await _produtoRepositorio.Adicionar(produtoDb);
        }

        public async Task<ProdutoDTOResponse?> AtualizarProduto(ProdutoDTORequest produtoDto)
        {
            var produtoDb = await _produtoRepositorio.ListarPorId(produtoDto.Id);

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

            var produtoAtualizado = await _produtoRepositorio.Atualizar(produtoDb);

            return _produtoMapper.ToDtoResponse(produtoAtualizado);
        }

        public async Task<ProdutoDTOResponse?> DetalhesProduto(int id)
        {
            var produto = await _produtoRepositorio.ListarPorId(id);
            if (produto == null)
                return null;
            return _produtoMapper.ToDtoResponse(produto);
        }

        public async Task<ProdutoDTOResponse?> ListarProdutoAtivoPorNome(string nomeProduto)
        {
            var produtoDb = await _produtoRepositorio.ListarProdutoPorNome(nomeProduto);
            if (produtoDb == null || produtoDb.Ativo == false)
                return null;
            return _produtoMapper.ToDtoResponse(produtoDb);
        }

        public async Task<List<ProdutoDTOResponse>?> ListarTodosProdutosAtivos()
        {
            var produtos = await _produtoRepositorio.ListarTodos();
            var produtosAtivos = produtos?.Where(x => x.Ativo).ToList();

            return (produtosAtivos != null && produtosAtivos.Count != 0) ?
                _produtoMapper.ToDtoResponseList(produtosAtivos) :
                null;
        }

        public async Task<bool> RemoverProduto(int idAluno)
        {
            var produtoDb = await _produtoRepositorio.ListarPorId(idAluno);
            if (produtoDb == null)
                return false;

            produtoDb.Ativo = false;

            _ = await _produtoRepositorio.Atualizar(produtoDb);

            return true;
        }
    }
}
