namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Perfis
{
    public class PerfilClienteDTOResponse
    {
        public int ClienteId { get; set; }
        public string Perfil { get; set; }
        public decimal Pontuacao { get; set; }
        public string Descricao { get; set; }
    }
}
