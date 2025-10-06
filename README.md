# 🪙 **DimDim Bank — 2º Checkpoint (DevOps Tools & Cloud Computing)**

> DimDim é uma aplicação financeira que gerencia **Clientes, Contas Correntes e Transações** em uma arquitetura **.NET 8 + Azure SQL**, totalmente hospedada no **Azure App Service**.  
Nesta entrega, realizamos o **deploy automatizado** via **Azure CLI** e **GitHub Actions**, com **monitoramento pelo Application Insights**.

---

## 📊 Stack técnica

| Camada | Tecnologia |
|---------|-------------|
| Linguagem | C# / .NET 8 |
| Banco de Dados | Azure SQL Database (PaaS) |
| Hospedagem | Azure App Service (Linux) |
| Automação | Azure CLI (Bash Scripts) |
| CI/CD | GitHub Actions |
| ORM | Entity Framework Core |
| Observabilidade | Azure Application Insights |
| Documentação API | Swagger |

---

## 🚀 Deploy na Azure (passo a passo)

O repositório contém **dois scripts principais**:  
um para criar o **banco de dados** e outro para implantar a **aplicação web**.

---

### 📁 Passo 1 — Banco de Dados (Azure SQL)

Crie o script, dê permissão e edite:

```bash
touch cli-scripts/create-sql-server.sh
chmod +x cli-scripts/create-sql-server.sh
nano cli-scripts/create-sql-server.sh
```

Cole o conteúdo abaixo (substitua `<SUA_SENHA_FORTE>` por uma senha real segura):

```bash
#!/bin/bash
set -e

RG="rg-dimdim"
LOCATION="brazilsouth"
SERVER_NAME="cp6-dimdimdb"
USERNAME="admsql"
PASSWORD="<SUA_SENHA_FORTE>"
DBNAME="DimDimDB"

echo ">> Criando o grupo de recursos: $RG..."
az group create --name "$RG" --location "$LOCATION" >/dev/null

echo ">> Criando o servidor SQL: $SERVER_NAME..."
az sql server create \
  -l "$LOCATION" -g "$RG" -n "$SERVER_NAME" \
  -u "$USERNAME" -p "$PASSWORD" \
  --enable-public-network true --minimal-tls-version 1.2 >/dev/null

echo ">> Criando o banco de dados: $DBNAME..."
az sql db create \
  -g "$RG" -s "$SERVER_NAME" -n "$DBNAME" \
  --service-objective Basic \
  --backup-storage-redundancy Local \
  --zone-redundant false >/dev/null

echo ">> Configurando a regra de firewall: AllowAll (0.0.0.0 - 255.255.255.255)"
az sql server firewall-rule create \
  -g "$RG" -s "$SERVER_NAME" \
  -n AllowAll --start-ip-address 0.0.0.0 --end-ip-address 255.255.255.255 >/dev/null

echo "====================================="
echo "Infra de banco criada com sucesso!"
echo "Servidor: ${SERVER_NAME}.database.windows.net"
echo "Banco:    ${DBNAME}"
echo "Usuário:  ${USERNAME}"
echo
echo "Connection string ADO.NET:"
echo "Server=tcp:${SERVER_NAME}.database.windows.net,1433;Database=${DBNAME};User ID=${USERNAME};Password=${PASSWORD};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
echo "====================================="
```

Execute o script:

```bash
bash cli-scripts/create-sql-server.sh
```

> Após criar o banco, acesse o **Azure Portal → SQL Database → Query Editor**  
> e aplique o script DDL do banco (arquivo `DimDim.sql`).

---

### 💭 Passo 2 — Aplicação (App Service + Insights + CI/CD)

Crie o script, dê permissão e edite:

```bash
touch cli-scripts/deploy-dimdim.sh
chmod +x cli-scripts/deploy-dimdim.sh
nano cli-scripts/deploy-dimdim.sh
```

Cole o conteúdo abaixo (ajuste a senha e o nome do repositório se necessário):

```bash
#!/bin/bash
set -e

export RESOURCE_GROUP_NAME="rg-dimdim"
export WEBAPP_NAME="web-dimdim-valornull"
export APP_SERVICE_PLAN="plan-dimdim-b1"
export LOCATION="brazilsouth"
export RUNTIME="DOTNETCORE|8.0"
export GITHUB_REPO_NAME="valor-null/CP6-Devops"
export BRANCH="main"
export APP_INSIGHTS_NAME="ai-dimdim"

export DB_SERVER_NAME="cp6-dimdimdb"
export DB_NAME="DimDimDB"
export DB_USER="admsql"
export DB_PASSWORD="<SUA_SENHA_FORTE>"

export ADO_NET="Server=tcp:${DB_SERVER_NAME}.database.windows.net,1433;Database=${DB_NAME};User ID=${DB_USER};Password=${DB_PASSWORD};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

echo "Iniciando o deploy da aplicação e infraestrutura..."

az monitor app-insights component create \
  --app "$APP_INSIGHTS_NAME" \
  --location "$LOCATION" \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --application-type web >/dev/null

az appservice plan create \
  --name "$APP_SERVICE_PLAN" \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --location "$LOCATION" \
  --sku B1 \
  --is-linux >/dev/null

az webapp create \
  --name "$WEBAPP_NAME" \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --plan "$APP_SERVICE_PLAN" \
  --runtime "$RUNTIME" >/dev/null

az resource update \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --namespace Microsoft.Web \
  --resource-type basicPublishingCredentialsPolicies \
  --name scm \
  --parent sites/"$WEBAPP_NAME" \
  --set properties.allow=true >/dev/null

CONNECTION_STRING=$(az monitor app-insights component show \
  --app "$APP_INSIGHTS_NAME" \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --query connectionString -o tsv)

az webapp config appsettings set \
  --name "$WEBAPP_NAME" \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --settings \
    APPLICATIONINSIGHTS_CONNECTION_STRING="$CONNECTION_STRING" \
    ASPNETCORE_ENVIRONMENT="Production" \
    ASPNETCORE_URLS="http://0.0.0.0:8080" \
    WEBSITES_PORT="8080" >/dev/null

az webapp config connection-string set \
  --name "$WEBAPP_NAME" \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --settings DefaultConnection="$ADO_NET" \
  --connection-string-type SQLAzure >/dev/null

az webapp config set \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --name "$WEBAPP_NAME" \
  --always-on true >/dev/null

az webapp restart \
  --name "$WEBAPP_NAME" \
  --resource-group "$RESOURCE_GROUP_NAME" >/dev/null

az monitor app-insights component connect-webapp \
  --app "$APP_INSIGHTS_NAME" \
  --web-app "$WEBAPP_NAME" \
  --resource-group "$RESOURCE_GROUP_NAME" >/dev/null

az webapp deployment github-actions add \
  --name "$WEBAPP_NAME" \
  --resource-group "$RESOURCE_GROUP_NAME" \
  --repo "$GITHUB_REPO_NAME" \
  --branch "$BRANCH" \
  --login-with-github

echo "====================================="
echo "Deploy concluído com sucesso!"
echo "Site:     https://${WEBAPP_NAME}.azurewebsites.net"
echo "Swagger:  https://${WEBAPP_NAME}.azurewebsites.net/swagger/index.html"
echo "====================================="
```

Execute o script:

```bash
bash deploy-dimdim.sh
```

Após o deploy:  
acesse 👉 **https://web-dimdim-valornull.azurewebsites.net/swagger/index.html**

---

## 🧾 Inserts de Teste (API)

Use o **Swagger** ou o **Razor Pages** para testar a API.  
A sequência ideal para evitar erros de relacionamento (FK) é:

### 👤 Cliente — POST `/api/clientes`
```json
{
  "nome": "Maria Oliveira",
  "cpf": "12345678900",
  "email": "maria.oliveira@example.com"
}

```

### 🏦 Conta Corrente — POST `/api/contas`
```json
{
  "idCliente": 1,
  "numeroConta": "12345-6",
  "tipoConta": "Corrente"
}

```
### 💳 Transação 
## POST `/Transacoes/Depositar`
```json
{
  "idConta": 1,
  "valor": 250.00
}
```
## POST `/Transacoes/Sacar`
```json
{
  "idConta": 1,
  "valor": 100.00
}
```
## POST `/Transacoes/Transferir`
```json
{
  "idContaOrigem": 1,
  "idContaDestino": 2,
  "valor": 300.00
}

```
---
## ☀️ Modelo de Dados — Relações

> O banco foi projetado seguindo uma estrutura relacional simples e eficiente:

Cliente (1) ────< (N) ContaCorrente

ContaCorrente (1) ────< (N) Transacao

---

## 📊 Application Insights

O monitoramento está ativo via **Azure Application Insights**, permitindo acompanhar:
- Requisições e latência
- Logs e falhas
- Métricas de desempenho da API

---
## 👥 Equipe:

- ⭐️ Valéria Conceição Dos Santos — RM: 557177
- ⭐️ Mirela Pinheiro Silva Rodrigues — RM: 558191
- ⭐️ Guilherme Romanholi Santos — RM: 557462


