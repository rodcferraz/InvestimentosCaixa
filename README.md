# Projeto Investimento Caixa – Guia de Instalação, Execução e Uso

Este documento descreve como **restaurar o projeto**, **executá-lo localmente via Docker** e **acessar o Swagger** para testes dos endpoints.

---

## 1. Pré-requisitos

Antes de iniciar, certifique-se de ter instalado:

- **.NET 8 SDK**
- **Docker** + Docker Desktop
- **Git** (opcional)

---

## 2. Restaurando o Projeto

Após clonar o repositório, execute:

dotnet restore

dotnet build

dotnet run --project InvestimentosCaixa.Api

---

## 3. Executar projeto via Docker

cd ./InvestimentosCaixa/InvestimentosCaixa.Api

docker build -t investimentos-caixa-api .

docker run -d -p 8080:8080 --name investimentos-caixa-api-container investimentos-caixa-api


---

## 4. Acessando o Swagger

http://localhost:8080/swagger

---

## 5. Gerar Bearer token

Acesse o endpoint /login 
Utilize no header "Authorization : Bearer {token}"
