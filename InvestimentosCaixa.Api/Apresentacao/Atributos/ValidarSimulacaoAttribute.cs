using InvestimentosCaixa.Api.Apresentacao.Filtros;
using Microsoft.AspNetCore.Mvc;

namespace InvestimentosCaixa.Api.Apresentacao.Atributos
{
    public class ValidarSimulacaoAttribute : TypeFilterAttribute
    {
        public ValidarSimulacaoAttribute() 
            : base(typeof(ValidarSimulacaoFiltro)) { }
    }
}
