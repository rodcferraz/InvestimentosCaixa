using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    public class InvestimentoMapper : IInvestimentoMapper
    {
        public Investimento ToBaseEntity(InvestimentoDTOBaseRequest investimentoDto)
        {

            return new Investimento
            {
                IdCliente = investimentoDto.IdCliente,
                IdProduto = investimentoDto.IdProduto,
                Valor = investimentoDto.Valor,
                Data = DateTime.UtcNow
            };
        }

        public List<InvestimentoDTOResponse> ToDtoResponseList(List<Investimento> investimentos)
        {
            return investimentos.Select(i =>
            {
                return new InvestimentoDTOResponse
                {
                    Id = i.Id,
                    Valor = i.Valor,
                    Tipo = ((TipoProdutoEnum)i.Produto.Tipo).ToString(),
                    Rentabilidade = i.Produto.Rentabilidade,
                    Data = i.Data.ToString("yyyy-MM-dd")
                };
            }).ToList();
        }
    }
}
