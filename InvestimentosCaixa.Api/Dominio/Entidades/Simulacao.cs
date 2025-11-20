using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Dominio.Entidades
{
    /// <summary>
    /// Entidade que representa uma simulação no sistema de investimentos
    /// </summary>
    public class Simulacao
    {
        /// <summary>
        /// Id do produto que será a chave primária
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Id do cliente que está realizando a simulação e ë a chave estrangeira
        /// </summary>
        [Required]
        public int IdCliente { get; set; }

        /// <summary>
        /// Id do produto em que será investido na simulação e é a chave estrangeira
        /// </summary>
        [Required]
        public int IdProduto { get; set; }

        /// <summary>
        /// Valor a ser investido na simulação
        /// </summary>
        [Required]
        public decimal ValorInvestido { get; set; }

        /// <summary>
        /// Prazo em meses para a simulação do investimento
        /// </summary>
        [Required]
        public int PrazoMeses { get; set; }

        /// <summary>
        /// Data de simulçação do investimento
        /// </summary>
        public DateTime DataSimulacao { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Produto Produto { get; set; }
    }
}
