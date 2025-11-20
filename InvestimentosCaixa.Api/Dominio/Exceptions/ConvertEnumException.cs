namespace InvestimentosCaixa.Api.Dominio.Exceptions
{
    /// <summary>
    /// Lança erro de exceção quando a conversão de um valor para enum falha
    /// </summary>
    public class ConvertEnumException : Exception
    {
        public ConvertEnumException(Type tipoEnum, object valorInvalido) 
            : base(MensagemErroConversaoEnum(tipoEnum, valorInvalido))
        {

        }

        public static string MensagemErroConversaoEnum(Type tipoEnum, object valorInvalido)
        {
            var valorValido = Enum.GetValues(tipoEnum);
            var valoresString = valorValido.Cast<object>().Select(v => $"{v} ({(int)v})");
            return $"Valor '{valorInvalido}' não é válido para o enum {tipoEnum.Name}. " +
                   $"Valores válidos: {string.Join(", ", valoresString)}";
        }
    }
}
