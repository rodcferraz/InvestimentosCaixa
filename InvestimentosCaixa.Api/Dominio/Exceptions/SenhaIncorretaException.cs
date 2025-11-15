namespace InvestimentosCaixa.Api.Dominio.Exceptions
{
    public class SenhaIncorretaException : Exception
    {
        public SenhaIncorretaException(string erroMensagem) 
            : base(erroMensagem) { }
    }
}
