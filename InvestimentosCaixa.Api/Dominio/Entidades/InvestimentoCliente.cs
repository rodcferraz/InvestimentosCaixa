using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Dominio.Entidades
{
    public class InvestimentoCliente
    {
        [Required]
        public int InvestimentoId { get; set; }
        [Required]
        public int ClienteId { get; set; }
        [Required]
        public int ProdutoId { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Investimento Investimento { get; set; }

        public InvestimentoCliente()
        {
            Cliente = new Cliente();
            Produto = new Produto();
            Investimento = new Investimento();
        }
    }
}
