namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos
{
    /// <summary>
    /// Resposta para o perfil de recomendação de produto para o cliente
    /// </summary>
    public class ProdutoRecomendadoDTOResponse
    {
        /// <summary>
        /// Id do produto recomendado
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do produto recomendado
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Tipo do produto recomendado
        /// </summary>
        public string Tipo { get; set; }

        /// <summary>
        /// Rentabilidade do produto recomendado
        /// </summary>
        public decimal Rentabilidade { get; set; }

        /// <summary>
        /// Risco do produto recomendado
        /// </summary>
        public string Risco { get; set; }
    }
}
