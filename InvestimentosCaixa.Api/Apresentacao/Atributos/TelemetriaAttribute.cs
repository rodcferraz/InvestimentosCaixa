using InvestimentosCaixa.Api.Apresentacao.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Apresentacao.Atributos
{
    public class TelemetriaAttribute : TypeFilterAttribute
    {
        public TelemetriaAttribute()
            : base(typeof(TelemetriaFiltro)) { }
    }
}
