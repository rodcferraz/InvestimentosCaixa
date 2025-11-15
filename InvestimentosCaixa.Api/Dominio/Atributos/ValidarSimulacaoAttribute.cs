using InvestimentosCaixa.Api.Dominio.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Dominio.Atributos
{
    public class ValidarSimulacaoAttribute : TypeFilterAttribute
    {
        public ValidarSimulacaoAttribute() 
            : base(typeof(ValidarSimulacaoFiltro)) { }
    }
}
