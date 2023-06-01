provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "testGroupT"
  location = "Poland Central"
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
