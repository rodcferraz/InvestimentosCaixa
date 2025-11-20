using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Aplicacao.DTOs.Produtos
{
    /// <summary>
    /// Requisição para cadastro e atualização de produto
    /// </summary>
    public class ProdutoDTOBaseRequest
    {
        /// <summary>
        /// Nome do produto
        /// </summary>
        [Required(ErrorMessage = "Campo 'Nome' é obrigatório")]
        public string Nome { get; set; }

        /// <summary>
        /// Tipo de produto
        /// Valores: 
        /// TesouroSelic,
        /// CDB,
        /// LCI,
        /// LCA,
        /// TesouroIPCA,
        /// Fundo,
        /// Acoes,
        /// ETFs,
        /// Criptomoeda
        /// </summary>
        [Required(ErrorMessage = "Campo 'Tipo' é obrigatório")]
        public string Tipo { get; set; }

        /// <summary>
        /// Rentabilidade do produto
        /// </summary>
        [Required(ErrorMessage = "Campo 'Rentabilidade' é obrigatório")]
        public decimal Rentabilidade { get; set; }

        /// <summary>
        /// Risco associado ao produto
        /// Baixo,
        /// Moderado,
        /// Alto
        /// </summary>
        [Required(ErrorMessage = "Campo 'Risco' é obrigatório")]
        public string Risco { get; set; }
    }
}
