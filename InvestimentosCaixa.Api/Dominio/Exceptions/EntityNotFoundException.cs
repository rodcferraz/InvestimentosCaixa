namespace InvestimentosCaixa.Api.Dominio.Exceptions
{
    /// <summary>
    /// Lança exceção quando uma entidade não for encontrada no sistema
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message) { }
    }
}
