using System.ComponentModel.DataAnnotations;

namespace InvestimentosCaixa.Api.Dominio.Entidades
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string Nome { get; set; }

        [Required]
        public int Tipo { get; set; }

        [Required]
        public decimal Rentabilidade { get; set; }

        [Required]
        public int Risco { get; set; }

        public bool Ativo { get; set; } = true;
    }
}
