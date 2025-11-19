using InvestimentosCaixa.Api;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.ClienteControlerTestes.Mapper;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.SimulacaoControllerTestes.Mapper;
using InvestimentosCaixa.Testes.TestesIntegracao.Controllers.SimulacaoControllerTestes.Servicos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InvestimentosCaixa.Testes.TestesIntegracao.Controllers
{
    public class WebApplicationFactoryCustomizado : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove configuração real do DbContext
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<InvestimentosCaixaDbContext>)
                );

                if (descriptor != null)
                    services.Remove(descriptor);

                // === 1) Criar conexão única e abrir ===
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                // === 2) Registrar conexão como Singleton ===
                services.AddSingleton(connection);

                // === 3) Usar essa mesma conexão no DbContext ===
                services.AddDbContext<InvestimentosCaixaDbContext>((provider, options) =>
                {
                    var conn = provider.GetRequiredService<SqliteConnection>();
                    options.UseSqlite(conn);
                });

                // Fakes e Auth
                services.AddAuthentication("Teste")
                    .AddScheme<AuthenticationSchemeOptions, TesteAuthHandler>("Teste", null);

                services.PostConfigureAll<AuthorizationOptions>(options =>
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder("Teste")
                        .RequireAuthenticatedUser()
                        .Build();
                });

                //ReplaceService<IClienteMapper, ClienteMapperFake>(services);
                //ReplaceService<ISimulacaoMapper, SimulacaoMapperFake>(services);
                //ReplaceService<ISimulacaoServico, SimulacaoServicoNuloFake>(services);
                //ReplaceService<ISimulacaoServico, SimulacaoServicoErroFake>(services);

                // === 4) Criar tabelas ===
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();
                db.Database.EnsureCreated(); // Agora cria TUDO corretamente
            });
            //builder.ConfigureServices(services =>
            //{
            //    // Remove DB real
            //    var dbDescriptor = services.SingleOrDefault(
            //        d => d.ServiceType == typeof(DbContextOptions<InvestimentosCaixaDbContext>)
            //    );

            //    if (dbDescriptor != null)
            //        services.Remove(dbDescriptor);

            //    // Adiciona DB em memória
            //    services.AddDbContext<InvestimentosCaixaDbContext>((provider, options) =>
            //    {
            //        var conn = provider.GetRequiredService<SqliteConnection>();
            //        options.UseSqlite(conn);
            //    });

            //    // Substituir autenticação para permitir testes com [Authorize]
            //    services.AddAuthentication("Teste")
            //            .AddScheme<AuthenticationSchemeOptions, TesteAuthHandler>("Teste", null);

            //    // Substituir Auth real como padrão
            //    services.PostConfigureAll<AuthorizationOptions>(options =>
            //    {
            //        options.DefaultPolicy = new AuthorizationPolicyBuilder("Teste")
            //                .RequireAuthenticatedUser()
            //                .Build();
            //    });

            //    ReplaceService<IClienteMapper, ClienteMapperFake>(services);
            //    ReplaceService<ISimulacaoMapper, SimulacaoMapperFake>(services);
            //    // ReplaceService<ITelemetriaServico, TelemetriaFake>(services);


            //    // Criar banco
            //    //builder.Configure(app =>
            //    //{
            //    //    using var scope = app.ApplicationServices.CreateScope();
            //    //    var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();
            //    //    db.Database.OpenConnection();
            //    //    db.Database.EnsureCreated();
            //    //});
            //    // === CRIAR BANCO AQUI ===
            //    var sp = services.BuildServiceProvider();
            //    using var scope = sp.CreateScope();
            //    var db = scope.ServiceProvider.GetRequiredService<InvestimentosCaixaDbContext>();
            //    db.Database.EnsureCreated(); // Agora cria TUDO corretamente
            //});
        }

        public  void ReplaceService<T, TImpl>(IServiceCollection services)
            where T : class
            where TImpl : class, T
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(T));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddSingleton<T, TImpl>();
        }
    }
}
