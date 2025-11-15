using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace InvestimentosCaixa.Api.Infraestrutura.Repossitorios
{
    public class GenericoRepositorio<T> : IGenericoRepositorio<T> where T : class
    {
        private readonly InvestimentosCaixaDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericoRepositorio(InvestimentosCaixaDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();

        }
        public async Task AdicionarAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> ListarTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> ListarPorIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> AtualizarAsync(T aluno)
        {
            _dbSet.Update(aluno);
            await _context.SaveChangesAsync();
            return aluno;
        }

    }
}
