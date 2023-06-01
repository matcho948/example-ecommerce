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
  }
}
