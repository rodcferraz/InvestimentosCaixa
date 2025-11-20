using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Dominio.Entidades
{
    /// <summary>
    /// Entidade que representa um investimento no sistema de investimentos
    /// </summary>
    public class Investimento
    {
        /// <summary>
        /// Id do investimento que será a chave primária
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Id do cliente que será a chave estrangeira
        /// </summary>
        [Required]
        public int IdCliente { get; set; }

        /// <summary>
        /// Id do produto que será a chave estrangeira
        /// </summary>
        [Required]
        public int IdProduto { get; set; }

        /// <summary>
        /// Valor do investimento
        /// </summary>
        [Required]
        public decimal Valor { get; set; }

        /// <summary>
        /// Data realizada do investimento
        /// </summary>
        [Required]
        public DateTime Data { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Produto Produto { get; set; }
    }
}
