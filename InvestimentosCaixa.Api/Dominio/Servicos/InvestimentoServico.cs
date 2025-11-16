using InvestimentosCaixa.Api.Aplicacao.DTOs.Investimentos;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace InvestimentosCaixa.Api.Dominio.Servicos
{
    public class InvestimentoServico : IInvestimentoServico
    {
        private readonly IInvestimentoRepositorio _investimentoRepositorio;
        private readonly IInvestimentoMapper _investimentoMapper;

        public InvestimentoServico(
            IInvestimentoRepositorio investimentoRepositorio,
            IInvestimentoMapper investimentoMapper)
        {
            _investimentoRepositorio = investimentoRepositorio;
            _investimentoMapper = investimentoMapper;
        }

        public async Task CadastrarInvestimentoAsync(InvestimentoDTOBaseRequest investimentoDto)
        {
            var investimentoEntity = _investimentoMapper.ToBaseEntity(investimentoDto);

            await _investimentoRepositorio.AdicionarAsync(investimentoEntity);
        }

        public async Task<List<InvestimentoDTOResponse>> ListarInvestimentosPorClienteAsync(int idCliente)
        {
            var investimentos = await _investimentoRepositorio.ListarInvestimentosPorClienteAsync(idCliente);

            return investimentos.IsNullOrEmpty() ?
                    Enumerable.Empty<InvestimentoDTOResponse>().ToList() :
                    _investimentoMapper.ToDtoResponseList(investimentos);
        }
    }
}
