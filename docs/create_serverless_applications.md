# Create Serverless Applications

## Create Azure App Service Web Apps

* create an Azure App Service Web App
* enable diagnostics logging
* deploy code to a web app
* configure web app settings
* implement autoscaling rules (schedule, operational/system metrics)

## Implement Azure functions

Azure functions are serverless code offerings. The structure is to have a function app at the top level, with azure functions encapsulated by the function app.

These can be on an app service plan, which reserves a vm for thier use, or consumption plan where they are spun up and down based on usage. app service is a higher price with higher availability, while consumption plan can scale to meet demand, and will likely be cheaper for burst workflows, but may have some start up time if they haven't been triggered for a long time.

### Creating an azure function using core tools

You can use the azure cli core tools to create a function in any directory. 

``` 
func init
```

That creates the function app, now to create the actual function you can use:

``` 
func new
```

Follow the prompts to create a new function app. It can then be deployed to azure using:

``` 
func azure functionapp publish <app_name>
```

Before it can be published, the function app needs to exist. To create it you can use these commands

```
RESOURCEGROUP=learn-e78bd1b2-0405-46e9-b23b-daea52d477ad
STORAGEACCT=learnstorage$(openssl rand -hex 5)
FUNCTIONAPP=learnfunctions$(openssl rand -hex 5)

az storage account create \
  --resource-group "$RESOURCEGROUP" \
  --name "$STORAGEACCT" \
  --kind StorageV2 \
  --location centralus

az functionapp create \
  --resource-group "$RESOURCEGROUP" \
  --name "$FUNCTIONAPP" \
  --storage-account "$STORAGEACCT" \
  --runtime node \
  --consumption-plan-location centralus
```

When using this publish any functions already present are stopped and deleted before the new version is deployed.

* implement input and output bindings for a function
* implement function triggers by using data operations, timers, and webhooks
* implement Azure Durable Functions