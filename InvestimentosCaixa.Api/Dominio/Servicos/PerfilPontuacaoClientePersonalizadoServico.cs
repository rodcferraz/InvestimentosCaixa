using InvestimentosCaixa.Api.Aplicacao.DTOs.Perfis;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    public class PerfilPontuacaoClientePersonalizadoServico : IPerfilPontuacaoClienteServico
    {
        public int GerarPerfilCarteiraCliente(decimal totalInvestido)
        {
            if (totalInvestido <= 5000) return 10;
            else if (totalInvestido <= 20000) return 30;
            else if (totalInvestido <= 50000) return 50;
            else if (totalInvestido <= 100000) return 80;
            else return 100;
        }

        public int GerarPerfilMovimentacoesaCliente(int quantidadeMovimentacoes)
        {
            if (quantidadeMovimentacoes <= 2) return 20;
            else if (quantidadeMovimentacoes <= 5) return 50;
            else return 80;
        }

        public int GerarPerfilLiquidezCliente(PerfilRiscoClienteEnum liquidez)
        {
            var pontuacao = 0;

            if (liquidez == PerfilRiscoClienteEnum.Conservador) return 20;
            else if (liquidez == PerfilRiscoClienteEnum.Moderado) return 50;
            else if (liquidez == PerfilRiscoClienteEnum.Agressivo) return 80;

            return pontuacao;
        }
    }
}
