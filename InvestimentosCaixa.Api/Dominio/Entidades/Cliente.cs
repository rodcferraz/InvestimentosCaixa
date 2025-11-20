using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Dominio.Entidades
{
    /// <summary>
    /// Entidade que representa um cliente no sistema de investimentos
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// Id do cliente que será a chave da entidade
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do cliente
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        /// <summary>
        /// Email do cliente
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Senha hash do cliente
        /// </summary>
        [Required]
        [StringLength(255)]
        public string SenhaHash { get; set; }

        /// <summary>
        /// Perfil de risco do cliente representado pela liquidez
        /// </summary>
        [Required]
        public int Liquidez { get; set; }

        /// <summary>
        /// Status do cliente (ativo ou inativo)
        /// </summary>
        public bool Ativo { get; set; } = true;
        public virtual ICollection<Simulacao> Simulacoes { get; set; }
        public virtual ICollection<Investimento> Investimentos { get; set; }

        public Cliente()
        {
            Simulacoes = new HashSet<Simulacao>();
            Investimentos = new HashSet<Investimento>();
        }
    }
}
