namespace InvestimentosCaixa.Api.Dominio.Exceptions
{
    /// <summary>
    /// Lança exceção quando a senha informada pelo o usuário estiver incorreta
    /// </summary>
    public class SenhaIncorretaException : Exception
    {
        public SenhaIncorretaException(string erroMensagem) 
            : base(erroMensagem) { }
    }
}
