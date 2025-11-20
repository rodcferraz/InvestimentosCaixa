using System.ComponentModel;
using System.Reflection;

namespace InvestimentosCaixa.Api.Dominio.Enums
{
    /// <summary>
    /// Extensões para Enums
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Exibe a descrição do Enum
        /// </summary>
        /// <param name="valor">Classe de enum estendido</param>
        /// <returns>Descrição do enum</returns>
        public static string ExibirDescricao(this Enum valor)
        {
            var campo = valor.GetType().GetField(valor.ToString());
            var atributo = (DescriptionAttribute)campo?
                .GetCustomAttribute(typeof(DescriptionAttribute));

            return atributo?.Description ?? valor.ToString();
        }
    }
}
