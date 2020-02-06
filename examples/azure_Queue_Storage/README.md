create a storage account using the CLI

Find the correct tenant to use for this example

```
az account list
```

az storage account create --name [unique-name] -g learn-aae3bdde-9365-44f8-a878-ae24f5089545 --kind StorageV2 --sku Standard_LRS

az storage account create --name rabqueue -g learn-aae3bdde-9365-44f8-a878-ae24f5089545 --kind StorageV2 --sku Standard_LRS

To get access token
az storage account keys list --name rabqueue

Creating a c# project on command line

```
dotnet new console -n QueueApp
cd QueueApp
dotnet build
```

get connection string

```
az storage account show-connection-string --name rabqueue --resource-group learn-aae3bdde-9365-44f8-a878-ae24f5089545
```

add queue package 

```
dotnet add package WindowsAzure.Storage
```

Check results:

```
az storage message peek --queue-name newsqueue --connection-string "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=rabqueue;AccountKey=4FW1XFTTGK9kRN8uF2CVIf58vEDcQ+4z3jBIyDRJNQ612aRqH7Z8ml/Dxa7vW3HtcWKpi7Jpo0R1CgbdZB74bg=="
```

Put storage account connect string as env var and you don't have to pass it

``` bash
export AZURE_STORAGE_CONNECTION_STRING="DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=rabqueue;AccountKey=4FW1XFTTGK9kRN8uF2CVIf58vEDcQ+4z3jBIyDRJNQ612aRqH7Z8ml/Dxa7vW3HtcWKpi7Jpo0R1CgbdZB74bg=="
```

Then you can run it using this much simpler command

```
az storage message peek --queue-name newsqueue
```