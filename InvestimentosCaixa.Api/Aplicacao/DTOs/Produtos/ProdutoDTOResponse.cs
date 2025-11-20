namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos
{
    /// <summary>
    /// Resposta ao criar ou atualizar um produto
    /// </summary>
    public class ProdutoDTOResponse
    {
        /// <summary>
        /// Id do produto
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do produto
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Tipo do produto 
        /// </summary>
        public string Tipo { get; set; }

        /// <summary>
        /// Rentabilidade do produto
        /// </summary>
        public decimal Rentabilidade { get; set; }

        /// <summary>
        /// Risco associado ao produto
        /// </summary>
        public string Risco { get; set; }
    }
}
