using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Dominio.Entidades
{
    public class Simulacao
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int IdCliente { get; set; }
        [Required]
        public int IdProduto { get; set; }
        [Required]
        public decimal ValorInvestido { get; set; }
        [Required]
        public int PrazoMeses { get; set; }
        public DateTime DataSimulacao { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Produto Produto { get; set; }
    }
}
