using InvestimentosCaixa.Api.Dominio.Mappers;
using InvestimentosCaixa.Api.Dominio.Mappers.Interfaces;
using InvestimentosCaixa.Api.Dominio.Repositorios.Interfaces;
using InvestimentosCaixa.Api.Dominio.Servicos;
using InvestimentosCaixa.Api.Dominio.Servicos.Interfaces;
using InvestimentosCaixa.Api.Infraestrutura.Data.Context;
using InvestimentosCaixa.Api.Infraestrutura.Repositorios;
using InvestimentosCaixa.Api.Infraestrutura.Repossitorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
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

            builder.Services.AddScoped(typeof(IGenericoRepositorio<>), typeof(GenericoRepositorio<>));
            builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
            builder.Services.AddScoped<IProdutoServico, ProdutoServico>();
            builder.Services.AddScoped<IProdutoMapper, ProdutoMapper>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1"); });

            app.MapControllers();

            app.Run();
        }
    }
}
