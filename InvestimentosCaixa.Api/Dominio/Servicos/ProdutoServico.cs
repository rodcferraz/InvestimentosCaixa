using InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    /// <summary>
    /// Serviço responsável por gerenciar operações relacionadas a produtos.
    /// </summary>
    public class ProdutoServico : IProdutoServico
    {
        private readonly IProdutoMapper _produtoMapper;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ILogger<ProdutoServico> _logger;

        public ProdutoServico(IProdutoMapper produtoMapper,
                              IProdutoRepositorio produtoRepositorio,
                              ILogger<ProdutoServico> logger)
        {
            _produtoMapper = produtoMapper;
            _produtoRepositorio = produtoRepositorio;
            _logger = logger;
        }

        /// <summary>
        /// Adiciona produto informado
        /// </summary>
        /// <param name="produtoDto">Requisição para cadastro de produto</param>
        /// <returns>Id do produto gerado</returns>
        public async Task<int> AdicionarProdutoAsync(ProdutoDTOBaseRequest produtoDto)
        {
            var produtoDb = _produtoMapper.ToBaseEntity(produtoDto);

            _ = await _produtoRepositorio.AdicionarAsync(produtoDb);

            _logger.LogInformation($"Produto {produtoDb.Nome} com Id {produtoDb.Id} cadastrado");

            return produtoDb.Id;
        }

        /// <summary>
        /// Atualiza produto informado
        /// </summary>
        /// <param name="produtoDto">Requisição para atualização de produto</param>
        /// <returns>Produto com campos atualizados</returns>
        /// <exception cref="ConvertEnumException">Lança exceção quando <see cref="RiscoProdutoEnum"/> 
        /// ou <see cref="TipoProdutoEnum"/> não foram informados corretamente</exception>
        public async Task<ProdutoDTOResponse?> AtualizarProdutoAsync(ProdutoDTORequest produtoDto)
        {
            var produtoDb = await _produtoRepositorio.ListarPorIdAsync(produtoDto.Id);

            if (produtoDb == null)
            {
                _logger.LogWarning($"Produto com Id {produtoDto.Id} não encontrado para atualização.");
                return null;
            }

            if (!Enum.TryParse(produtoDto.Risco, out RiscoProdutoEnum riscoProduto))
            {
                _logger.LogError($"Erro ao converter enum {nameof(RiscoProdutoEnum)} durante a atualização do produto {produtoDto.Nome}.");
                throw new ConvertEnumException(typeof(RiscoProdutoEnum), produtoDto.Risco);
            }

            if (!Enum.TryParse(produtoDto.Tipo, out TipoProdutoEnum tipoProduto))
            {
                _logger.LogError($"Erro ao converter enum {nameof(TipoProdutoEnum)} durante a atualização do produto {produtoDto.Nome}.");
                throw new ConvertEnumException(typeof(TipoProdutoEnum), produtoDto.Tipo);
            }

            produtoDb.Nome = produtoDto.Nome;
            produtoDb.Rentabilidade = produtoDto.Rentabilidade;
            produtoDb.Risco = (int) riscoProduto;
            produtoDb.Tipo =(int) tipoProduto;

            var produtoAtualizado = await _produtoRepositorio.AtualizarAsync(produtoDb);

            _logger.LogInformation($"Produto com Id {produtoDto.Id} atualizado com sucesso.");

            return _produtoMapper.ToDtoResponse(produtoAtualizado);
        }

        /// <summary>
        /// Exibe detalhes do produto pelo Id
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <returns>Dados do produto</returns>
        public async Task<ProdutoDTOResponse?> DetalhesProdutoAsync(int id)
        {
            var produto = await _produtoRepositorio.ListarPorIdAsync(id);
            if (produto == null || produto.Ativo == false) 
            { 
                _logger.LogWarning($"Produto com Id {id} não encontrado.");
                return null;
            }
                
            return _produtoMapper.ToDtoResponse(produto);
        }

        /// <summary>
        /// Listar todos os produtos ativos pelo nome
        /// </summary>
        /// <param name="nomeProduto">Nome do produto</param>
        /// <returns>Lista de produtos ativos pelo nome</returns>
        public async Task<ProdutoDTOResponse?> ListarProdutoAtivoPorNomeAsync(string nomeProduto)
        {
            var produtoDb = await _produtoRepositorio.ListarProdutoPorNome(nomeProduto);
            if (produtoDb == null || produtoDb.Ativo == false)
            {
                _logger.LogWarning($"Produto com nome {nomeProduto} não encontrado ou inativo.");
                return null;
            }
            
            _logger.LogInformation($"Produto com nome {nomeProduto} encontrado.");

            return _produtoMapper.ToDtoResponse(produtoDb);
        }

        /// <summary>
        /// Exibe produto ativo por tipo
        /// </summary>
        /// <param name="tipoProduto">Tipo do produto <see cref="TipoProdutoEnum"/></param>
        /// <returns>Produto encontrado</returns>
        /// <exception cref="ConvertEnumException">Lança exceção quando tipo informado não está mapeado em <see cref="TipoProdutoEnum"/></exception>
        public async Task<ProdutoDTOResponse?> ListarProdutoAtivoPorTipoAsync(string tipoProduto)
        {
            if (!Enum.TryParse(tipoProduto, out TipoProdutoEnum TipoProduto))
            {
                _logger.LogError($"Erro ao converter enum {nameof(TipoProdutoEnum)} durante a busca do produto por tipo {tipoProduto}.");
                throw new ConvertEnumException(typeof(TipoProdutoEnum), tipoProduto);
            }

            var produtoDb = await _produtoRepositorio.ListarProdutoPorTipo((int)TipoProduto);

            if (produtoDb == null)
            {
                _logger.LogWarning($"Produto com tipo {tipoProduto} não encontrado.");
                return null;
            }

            _logger.LogInformation($"Produto com tipo {tipoProduto} encontrado.");

            return _produtoMapper.ToDtoResponse(produtoDb);
        }

        /// <summary>
        /// Listar todos os produtos ativos por perfil de cliente
        /// </summary>
        /// <param name="perfil">Perfil de cliente</param>
        /// <returns>Lista de produtos ativos que corres</returns>
        public async Task<List<ProdutoDTOResponse>> ListarProdutosAtivosPorPerfilAsync(int idPerfil)
        {
            var produtos = await this.ListarTodosProdutosAtivosAsync();

            if (produtos == null || produtos.Count == 0)
            {
                _logger.LogWarning($"Nenhum produto ativo encontrado para o perfil {idPerfil}.");
                return Enumerable.Empty<ProdutoDTOResponse>().ToList();
            }

            return (produtos == null || produtos.Count == 0) ? 
                    Enumerable.Empty< ProdutoDTOResponse >().ToList() :
                    produtos
                       .Where(x => 
                            x.Risco == ((RiscoProdutoEnum)idPerfil).ToString())
                       .ToList();
        }

        /// <summary>
        /// Listar todos os produtos ativos
        /// </summary>
        /// <returns>Lista de produtos ativos</returns>
        public async Task<List<ProdutoDTOResponse>?> ListarTodosProdutosAtivosAsync()
        {
            var produtos = await _produtoRepositorio.ListarTodosAsync();
            var produtosAtivos = produtos?.Where(x => x.Ativo).ToList();

            _logger.LogInformation($"Listagem de produtos ativos realizada com sucesso.");

            if (produtosAtivos == null || produtosAtivos.Count == 0)
            {
                return Enumerable.Empty<ProdutoDTOResponse>().ToList();
            }

            return _produtoMapper.ToDtoResponseList(produtosAtivos);
        }

        /// <summary>
        /// Remoção lógica do produto pelo Id
        /// </summary>
        /// <param name="idProduto">Id do produto</param>
        /// <returns>Confirmação de deleção lógica do produto</returns>
        public async Task<bool> RemoverProdutoAsync(int idProduto)
        {
            var produtoDb = await _produtoRepositorio.ListarPorIdAsync(idProduto);
            if (produtoDb == null)
            {
                _logger.LogWarning($"Produto com Id {idProduto} não encontrado para remoção.");
                return false;
            }

            produtoDb.Ativo = false;

            _ = await _produtoRepositorio.AtualizarAsync(produtoDb);

            _logger.LogInformation($"Produto {produtoDb} invativado com sucesso.");

            return true;
        }
    }
}
