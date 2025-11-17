using InvestimentosCaixa.Api.Configuracoes;
using System.Security.Cryptography;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    public class SegurancaServico
    {
        private readonly AppSettings _appSettings;
        public SegurancaServico(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public string CriptografarPasswordHash(string senha)
        {
            using var hmac = new HMACSHA256(
                System.Text.Encoding.UTF8.GetBytes(_appSettings.ChaveHash));

            var senhaBytes = System.Text.Encoding.UTF8.GetBytes(senha);
            var hashBytes = hmac.ComputeHash(senhaBytes);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
