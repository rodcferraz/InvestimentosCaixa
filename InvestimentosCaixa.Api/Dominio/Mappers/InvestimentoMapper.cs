using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Dominio.Entidades;
using InvestimentosCaixa.Api.Dominio.Enums;
using InvestimentosCaixa.Api.Dominio.Exceptions;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;

namespace InvestimentosCaixa.Api.Dominio.Mappers
{
    public class InvestimentoMapper : IInvestimentoMapper
    {
        public Investimento ToBaseEntity(InvestimentoDTOBaseRequest investimentoDto)
        {
            if (!Enum.TryParse(investimentoDto.Tipo, out TipoProdutoEnum tipoProduto))
            {
                throw new ConvertEnumException(typeof(TipoProdutoEnum), investimentoDto.Tipo);
            }

            return new Investimento
            {
                Tipo = (int)tipoProduto,
                Valor = investimentoDto.Valor,
                Rentabilidade = investimentoDto.Rentabilidade,
                Data = DateTime.UtcNow
            };
        }

        public List<InvestimentoDTOResponse> ToDtoResponseList(List<Investimento> investimentos)
        {
            return investimentos.Select(i =>
            {
                var produto = i.InvestimentosCliente
                    .Select(ic => ic.Produto)
                    .FirstOrDefault();

                return new InvestimentoDTOResponse
                {
                    Id = i.Id,
                    Valor = i.Valor,
                    Tipo = ((TipoProdutoEnum)produto.Tipo).ToString(),
                    Rentabilidade = produto.Rentabilidade,
                    Data = i.Data.ToString("yyyy-MM-dd")
                };
            }).ToList();
        }
    }
}
