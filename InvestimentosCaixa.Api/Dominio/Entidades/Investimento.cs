using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Dominio.Entidades
{
    public class Investimento
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Tipo { get; set; }
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public decimal Rentabilidade { get; set; }
        [Required]
        public DateTime Data { get; set; }
        public virtual ICollection<InvestimentoCliente> InvestimentosCliente { get; set; }

        public Investimento()
        {
            InvestimentosCliente = new HashSet<InvestimentoCliente>();
        }
    }
}
