provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "testGroupT"
  location = "North Europe"
}

terraform {
  backend "azurerm" {
    resource_group_name  = "MyResourceGroup"
    storage_account_name = "myresource17124"
    container_name       = "test"
    key                  = "prod.terraform.tfstate"
    access_key = "Cvo5/+XUlhisJMfCby1o5SqWdinai0sGps9vqQdGghq5iFzoILEhNrkpj6yyg2JvmWKFF82DF0vO+AStXCMf7g=="
  }
}

resource "azurerm_app_service_plan" "example" {
  name                = "myAppServicePlan"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku {
    tier = "Free"
    size = "F1"
  }
}

resource "azurerm_application_insights" "example" {
  name                = "example-appinsights421214211142"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  application_type    = "web"
}

resource "azurerm_app_service" "example" {
  name                = "mywebapp5431134432424545"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_app_service_plan.example.id
  
  site_config {
    dotnet_framework_version = "v5.0"
  }
  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = azurerm_application_insights.example.instrumentation_key
    "ConnectionStrings:DefaultConnection" = "Server=tcp:${azurerm_sql_server.example.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_sql_database.example.name};Persist Security Info=False;User ID=${azurerm_sql_server.example.administrator_login};Password=${azurerm_sql_server.example.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
  connection_string {
    name  = "DBConnectionString"
    type  = "SQLAzure"
    value = "Server=tcp:${azurerm_sql_server.example.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_sql_database.example.name};Persist Security Info=False;User ID=${azurerm_sql_server.example.administrator_login};Password=${azurerm_sql_server.example.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}

resource "azurerm_sql_server" "example" {
  name                         = "mywebappsqlserver123"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = "myWebAppLog1"
  administrator_login_password = "HnX3gI1*k5&@"
}

resource "azurerm_sql_database" "example" {
  name                = "example-database"
  resource_group_name = azurerm_resource_group.rg.name
  server_name         = azurerm_sql_server.example.name
  location            = azurerm_resource_group.rg.location
  edition             = "Basic"
}

