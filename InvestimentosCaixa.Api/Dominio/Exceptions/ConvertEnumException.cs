namespace InvestimentosCaixa.Api.Dominio.Exceptions
{
    public class ConvertEnumException : Exception
    {
        public ConvertEnumException(Type tipoEnum, object valorInvalido) 
            : base(MensagemErro(tipoEnum, valorInvalido))
        {

        }

        public static string MensagemErro(Type tipoEnum, object valorInvalido)
        {
            var valorValido = Enum.GetValues(tipoEnum);
            var valoresString = valorValido.Cast<object>().Select(v => $"{v} ({(int)v})");
            return $"Valor '{valorInvalido}' não é válido para o enum {tipoEnum.Name}. " +
                   $"Valores válidos: {string.Join(", ", valoresString)}";
        }
    }
}
