using System.ComponentModel;
using System.Reflection;

namespace InvestimentosCaixa.Api.Dominio.Enums
{
    public static class EnumExtensions
    {
        public static string ExibirDescricao(this Enum valor)
        {
            var campo = valor.GetType().GetField(valor.ToString());
            var atributo = (DescriptionAttribute)campo?
                .GetCustomAttribute(typeof(DescriptionAttribute));

            return atributo?.Description ?? valor.ToString();
        }
    }
}
