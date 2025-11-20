using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Dominio.Entidades
{
    /// <summary>
    /// Entidade que representa um produto no sistema de investimentos
    /// </summary>
    public class Produto
    {
        /// <summary>
        /// Id do produto que será a chave primária
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do produto
        /// </summary>
        [Required]
        [StringLength(15)]
        public string Nome { get; set; }

        /// <summary>
        /// Tipo do produto
        /// </summary>
        [Required]
        public int Tipo { get; set; }

        /// <summary>
        /// Rentabilidade do produto
        /// </summary>
        [Required]
        public decimal Rentabilidade { get; set; }

        /// <summary>
        /// Risco associado ao produto
        /// </summary>
        [Required]
        public int Risco { get; set; }

        /// <summary>
        /// Status do produto (ativo ou inativo)
        /// </summary>
        public bool Ativo { get; set; } = true;
        public virtual ICollection<Simulacao> Simulacoes { get; set; }
        public virtual ICollection<Investimento> Investimentos { get; set; }

        public Produto()
        {
            Simulacoes = new HashSet<Simulacao>();
            Investimentos = new HashSet<Investimento>();
        }
    }
}
