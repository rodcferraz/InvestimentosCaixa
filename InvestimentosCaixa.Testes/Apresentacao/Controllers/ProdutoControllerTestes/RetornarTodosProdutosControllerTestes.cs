namespace InvestimentosCaixa.Testes.Apresentacao.Controllers.ProdutoControllerTestes
{
    public class RetornarTodosProdutosControllerTestes : IClassFixture<ProdutoControllerFixture>
    {
        private readonly ProdutoControllerFixture _fixture;

        public RetornarTodosProdutosControllerTestes()
        {
            _fixture = new ProdutoControllerFixture();
        }
    }
}
