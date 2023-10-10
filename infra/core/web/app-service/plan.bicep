metadata description = 'Creates an Azure App Service plan.'

param name string
param location string = resourceGroup().location
param tags object = {}

@description('OS type of the plan. Defaults to "linux".')
param kind string = 'linux'

@description('SKU for the plan. Defaults to "F1".')
param sku string = 'F1'

resource plan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: name
  location: location
  tags: tags
  sku: {
    name: sku
  }
  kind: kind
  properties: {
    reserved: kind == 'linux' ? true : null
  }
}

output name string = plan.name
