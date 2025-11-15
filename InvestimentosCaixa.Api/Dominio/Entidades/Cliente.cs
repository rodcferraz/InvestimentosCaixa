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
        public decimal RendaMensal { get; set; }
        [Required]
        public decimal PercentualInvestimentoRenda { get; set; }
        [Required]
        public int PerfilDeclarado { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
