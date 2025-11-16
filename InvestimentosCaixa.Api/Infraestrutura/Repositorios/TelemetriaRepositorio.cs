using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;

namespace InvestimentosCaixa.Api.Infraestrutura.Repositorios
{
    public class TelemetriaRepositorio : 
        GenericoRepositorio<Telemetria>, ITelemetriaRepositorio
    {
        public TelemetriaRepositorio(InvestimentosCaixaDbContext context) 
            : base(context)
        {
        }
    }
}
