namespace InvestimentosCaixa.Api.Dominio.Servicos.Interfaces
{
    public interface ISegurancaServico
    {
        string CriptografarPasswordHash(string senha);
    }
}
