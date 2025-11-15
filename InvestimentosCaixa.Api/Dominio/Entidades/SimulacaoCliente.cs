using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Dominio.Entidades
{
    public class SimulacaoCliente
    {
        [Required]
        public int ClienteId { get; set; }
        [Required]
        public int ProdutoId { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Simulacao Simulacao { get; set; }

        public SimulacaoCliente()
        {
            Cliente = new Cliente();
            Produto = new Produto();
            Simulacao = new Simulacao();
        }
    }
}
