using System.ComponentModel;

namespace InvestimentosCaixa.Api.Dominio.Enums
{
    public enum PerfilRiscoClienteEnum
    {
        [Description("Perfil com baixa movimentação, foco em liquidez")]
        Conservador = 1,
        [Description("Perfil equilibrado entre segurança e rentabilidade.")]
        Moderado = 2,
        [Description("Perfil que busca por alta rentabilidade, maior risco")]
        Agressivo = 3
    }
}
