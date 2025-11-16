using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Dominio.Entidades
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Nome { get; set; }
        [Required]
        [StringLength(20)]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string SenhaHash { get; set; }
        [Required]
        public int Liquidez { get; set; }
        public bool Ativo { get; set; } = true;
        public virtual ICollection<SimulacaoCliente> SimulacoesCliente { get; set; }
        public virtual ICollection<InvestimentoCliente> InvestimentosCliente { get; set; }

        public Cliente()
        {
            SimulacoesCliente = new HashSet<SimulacaoCliente>();
            InvestimentosCliente = new HashSet<InvestimentoCliente>();
        }
    }
}
