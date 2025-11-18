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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace InvestimentosCaixa.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ➤ Add services to the container
            builder.Services.AddEndpointsApiExplorer();

            // ➤ Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Minha API",
                    Version = "v1"
                });

                // Definição do Bearer (mas sem exigir globalmente)
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Insira o token assim: Bearer {seu-token}",
                    Type = SecuritySchemeType.Http
                });

                // Apenas aplicar a segurança nos métodos com [Authorize]
                c.OperationFilter<SwaggerAuthorizeOperationFilter>();
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.MaxDepth = 64;
                });

            // ➤ Banco
            builder.Services.AddDbContext<InvestimentosCaixaDbContext>(options =>
                options.UseSqlite("Data Source=InvestimentoCaixa.db"));

            var appSettings = new AppSettings(builder.Configuration);
            builder.Services.AddSingleton(appSettings);

            // ➤ Injeção dos repos / serviços / mappers (seu código)
            builder.Services.AddScoped(typeof(IGenericoRepositorio<>), typeof(GenericoRepositorio<>));

            // Produtos
            builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
            builder.Services.AddScoped<IProdutoServico, ProdutoServico>();
            builder.Services.AddScoped<IProdutoMapper, ProdutoMapper>();

            // Clientes
            builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            builder.Services.AddScoped<IClienteServico, ClienteServico>();
            builder.Services.AddScoped<IClienteMapper, ClienteMapper>();

            // Simulacoes
            builder.Services.AddScoped<ISimulacaoRepositorio, SimulacaoRepositorio>();
            builder.Services.AddScoped<ISimulacaoServico, SimulacaoServico>();
            builder.Services.AddScoped<ISimulacaoMapper, SimulacaoMapper>();

            // Investimentos
            builder.Services.AddScoped<IInvestimentoRepositorio, InvestimentoRepositorio>();
            builder.Services.AddScoped<IInvestimentoServico, InvestimentoServico>();
            builder.Services.AddScoped<IInvestimentoMapper, InvestimentoMapper>();

            // Telemetrias
            builder.Services.AddScoped<ITelemetriaRepositorio, TelemetriaRepositorio>();
            builder.Services.AddScoped<ITelemetriaServico, TelemetriaServico>();
            builder.Services.AddScoped<ITelemetriaMapper, TelemetriaMapper>();

            // Perfis
            builder.Services.AddScoped<IGerarPerfilClienteServico, GerarPerfilClienteServico>();
            builder.Services.AddScoped<IPerfilPontuacaoClienteServico, PerfilPontuacaoClientePersonalizadoServico>();
            builder.Services.AddScoped<IPerfilRiscoClienteServico, PerfilRiscoClientePersonalizado>();
            builder.Services.AddScoped<ICalculoPerfilRiscoMapper, CalculoParaPerfilRiscoMapper>();

            // Segurança
            builder.Services.AddSingleton<JwtServico>();
            builder.Services.AddScoped<ISegurancaServico, SegurancaServico>();

            // Filtro
            builder.Services.AddScoped<ValidarSimulacaoFiltro>();

            // ➤ JWT Auth
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = appSettings.Issuer,
                        ValidAudience = appSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(appSettings.Key))
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // ➤ Pipeline
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1");
            });

            app.MapControllers();

            app.Run();
        }
    }
}
