using InvestimentosCaixa.Api.Aplicacao.Servicos;
using InvestimentosCaixa.Api.Aplicacao.Servicos.Interfaces;
using InvestimentosCaixa.Api.Apresentacao.Filtros;
using InvestimentosCaixa.Api.Configuracoes;
using InvestimentosCaixa.Api.Dominio.Mappers;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using InvestimentosCaixa.Api.Infraestrutura.Repositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using System.Text.Json.Serialization;

namespace InvestimentosCaixa.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha API", Version = "v1", Description = "Documentação da API com Swagger" }); });

            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.MaxDepth = 64; // opcional, aumenta limite
            });

            builder.Services.AddDbContext<InvestimentosCaixaDbContext>(options => 
                options
                    .UseSqlite("Data Source=InvestimentoCaixa.db"));

            var appSettings = new AppSettings(builder.Configuration);
            builder.Services.AddSingleton(appSettings);

            builder.Services.AddScoped(typeof(IGenericoRepositorio<>), typeof(GenericoRepositorio<>));

            //Produtos
            builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
            builder.Services.AddScoped<IProdutoServico, ProdutoServico>();
            builder.Services.AddScoped<IProdutoMapper, ProdutoMapper>();

            //Clientes
            builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            builder.Services.AddScoped<IClienteServico, ClienteServico>();
            builder.Services.AddScoped<IClienteMapper, ClienteMapper>();

            //Simulacoes
            builder.Services.AddScoped<ISimulacaoRepositorio, SimulacaoRepositorio>();
            builder.Services.AddScoped<ISimulacaoServico, SimulacaoServico>();
            builder.Services.AddScoped<ISimulacaoMapper, SimulacaoMapper>();

            //Investimentos
            builder.Services.AddScoped<IInvestimentoRepositorio, InvestimentoRepositorio>();
            builder.Services.AddScoped<IInvestimentoServico, InvestimentoServico>();
            builder.Services.AddScoped<IInvestimentoMapper, InvestimentoMapper>();

            //Telemetrias
            builder.Services.AddScoped<ITelemetriaRepositorio, TelemetriaRepositorio>();
            builder.Services.AddScoped<ITelemetriaServico, TelemetriaServico>();
            builder.Services.AddScoped<ITelemetriaMapper, TelemetriaMapper>();

            //Perfis
            builder.Services.AddScoped<IGerarPerfilClienteServico, GerarPerfilClienteServico>();
            builder.Services.AddScoped<IPerfilPontuacaoClienteServico, PerfilPontuacaoClientePersonalizadoServico>();
            builder.Services.AddScoped<IPerfilRiscoClienteServico, PerfilRiscoClientePersonalizado>(); 
            builder.Services.AddScoped<ICalculoPerfilRiscoMapper, CalculoPerfilRiscoMapper>();

            //JWT
            builder.Services.AddSingleton<JwtServico>();

            //Customização de mensagem de erro de Model
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var erros = context.ModelState
                        .Where(x => x.Value.Errors.Any())
                        .Select(x => new {
                            Campo = x.Key,
                            Mensagens = x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        });

                    return new BadRequestObjectResult(erros);
                };
            });

            builder.Services.AddScoped<ValidarSimulacaoFiltro>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var settings = builder.Configuration.GetSection("Jwt");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = appSettings.Issuer,
                    ValidAudience = appSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(appSettings.Key)
                    )
                };
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1"); });

            app.MapControllers();

            app.Run();
        }
    }
}
