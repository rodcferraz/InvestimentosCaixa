using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Api.Infraestrutura.Repositorios
{
    /// <summary>
    /// Repositório genétco para operações CRUD básicas
    /// </summary>
    public class GenericoRepositorio<T> : IGenericoRepositorio<T> where T : class
    {
        private readonly InvestimentosCaixaDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericoRepositorio(InvestimentosCaixaDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();

        }
        /// <summary>
        /// Adiciona uma nova entidade ao banco de dados de forma assíncrona
        /// </summary>
        public async Task<T> AdicionarAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Lista todas as entidades de forma assíncrona
        /// </summary>
        public virtual async Task<List<T>> ListarTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Lista por id a entidade de forma assíncrona
        /// </summary>
        public async Task<T?> ListarPorIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Atualiza uma entidade existente no banco de dados de forma assíncrona
        /// </summary>
        public async Task<T> AtualizarAsync(T entidade)
        {
            _dbSet.Update(entidade);
            await _context.SaveChangesAsync();
            return entidade;
        }

    }
}
