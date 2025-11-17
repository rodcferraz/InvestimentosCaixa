namespace InvestimentosCaixa.Api.Configuracoes
{
    public class AppSettings
    {
        private readonly IConfigurationRoot _settings;

        public string CalculoPerfilRisco => _settings["CalculoPerfilRisco"];
        public string Key => _settings["Jwt:Key"];
        public string Issuer => _settings["Jwt:Issuer"];
        public string Audience => _settings["Jwt:Audience"];
        public string ChaveHash => _settings["ChaveHash"];

        public AppSettings(IConfigurationRoot settings)
        {
            _settings = settings;
        }
    }
}
