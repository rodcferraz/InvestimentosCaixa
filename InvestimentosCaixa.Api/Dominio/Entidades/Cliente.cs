using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Dominio.Entidades
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string SenhaHash { get; set; }
        [Required]
        public int Liquidez { get; set; }
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
