# Reliable Messaging for Big With Event Hub

## Learning Objectives

* Create an Event Hub using the Azure CLI.
* Configure applications to send or receive messages through an Event Hub.
* Evaluate Event Hub performance using the Azure portal.

### About Event Hubs

Events (individual or batch) cannot exceed a total of `256 KB`
Event Producers are called publishers, and event consumers are called consumers/subscribers
Publishers can send events using HTTPS or Advanced Message Queuing Protocol (AMQP)

Event Hub subscribers can use two methods to process events

* EventHubReceiver - Simple method with limited management options
* EventProcessorHost - An efficient Method

Consumer groups can be used to segment different types of data streams, so they are isolated and not impacted by other streams/applications.

Standard Tier gets you 20 Consumer Groups and 1000 Brokered Connections as time of writing.

Namespaces in event hub encapsulate all the settings for that namespace, such as pricing tier and throughput, which cannot be changed once the namespace is created. Namespace names must be globally unique. They create a url with this pattern `namespace.servicebus.windows.net`

These properties are also available.
* Enable Kafka apps to be able to publish events to the event hub
* Enable availability Zone redundancy
* Enable Auto-Inflate and Auto-Inflate Max throughput units to scale up throughput units

### Create an Event Hub using the Azure CLI.



#### Create the event hub namespace
for conviencience we can setup shell defaults for resource group and location

```
az configure --defaults group=learn-1df06427-edfc-492a-ab7f-657a4a7c030b location=westus2
```

First step is to create an event hub namespace. 
``` bash
az eventhubs namespace create --name rablabeventhub --sku Standard
```

Note. --resource-group and --l (for location) are both required parameters, but since we set the default in az configure, we can omit it and let it use our default making this command shorter.

To make it easier later, we can set this name to be an environment variable in the session
``` bash
NS_NAME=rablabeventhub
```

You can get the connection string from the newly created event hub namespace using the authorization-rul command
``` bash
az eventhubs namespace authorization-rule keys list \
    --name RootManageSharedAccessKey \
    --namespace-name $NS_NAME
```

This should return a json block like this:
``` JSON
{
  "aliasPrimaryConnectionString": null,
  "aliasSecondaryConnectionString": null,
  "keyName": "RootManageSharedAccessKey",
  "primaryConnectionString": "Endpoint=sb://rablabeventhub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=1dWMzR3qJnJYRCRQh/Q5otTyiFnshEAfJ9WvlQk4Ano=",
  "primaryKey": "1dWMzR3qJnJYRCRQh/Q5otTyiFnshEAfJ9WvlQk4Ano=",
  "secondaryConnectionString": "Endpoint=sb://rablabeventhub.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=aBGEcFmICg3Trff6j1DUZ1l6Wa0uFJJI8+T/UAhoCDo=",
  "secondaryKey": "aBGEcFmICg3Trff6j1DUZ1l6Wa0uFJJI8+T/UAhoCDo="
}
```

PrimaryConnectionString and PrimaryKey will be needed to create the event hub.


#### Create the Event Hub

As a prereq lets set the event hub name as an envrionrnment variable as well
``` bash
HUB_NAME=rablabhub
```

After the namespace has been created you can create the event hub. With both the variables set in the environmet, it makes this command very clean.
```
az eventhubs eventhub create --name $HUB_NAME --namespace-name $NS_NAME
```

much like creating the namespace we can omit the location and resource group from this command and let it use the defaults.

Now that the event hub is created we can see the details using the show command

``` bash
az eventhubs eventhub show --namespace-name $NS_NAME --name $HUB_NAME
```

### Configure applications to send or receive messages through an Event Hub.

The minimum Event Hub Application Requirements for sending a message:

* Event hub namespace name
* Event hub name
* Shared Access Policy Name
* Primary Shared access key

The minimum Event Hub Application Requirements for receiving a message:

* Event hub namespace name
* Event hub name
* Shared Access Policy Name
* Primary Shared access key

Additionally: 
* Storage Account Name
* Storage Account Connection String
* Storage Account Container Name

If your receiver app stores the messeages in a blob storage that will have to be configure as well.

#### Creating a storage account to store received messages

set the storage account name as a variable to ease the following steps:
``` bash
STORAGE_NAME=rabeventstorage
```

Then create the storage account
```
az storage account create --name $STORAGE_NAME --sku Standard_RAGRS
```

Then we can get all the access keys using
``` bash
az storage account keys list --account-name $STORAGE_NAME
```

Which will return something like this:
``` JSON
[
  {
    "keyName": "key1",
    "permissions": "Full",
    "value": "T7b/yWPECiOmtaqg3ps5Osi0m3By7Cv/jtwB/ihi/SlVhw0xzpGSuV+srzpWPmAztmHespR+JsY9wIqskO6/Xw=="
  },
  {
    "keyName": "key2",
    "permissions": "Full",
    "value": "MYCrq5buUHJk6pNMv+eNU2DXv0NA1dZgMI6YIqmWO6GDKkpBN76Tm2VcFCneSiU1W03LhoLS/cVehFjoS52mtg=="
  }
]
```

To get the connection string we can use the show-connection-string keyword
``` bash
az storage account show-connection-string -n $STORAGE_NAME
```

Which gives us the full connection string
``` JSON
{
  "connectionString": "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=rabeventstorage;AccountKey=T7b/yWPECiOmtaqg3ps5Osi0m3By7Cv/jtwB/ihi/SlVhw0xzpGSuV+srzpWPmAztmHespR+JsY9wIqskO6/Xw=="
}
```

Now we can create a container to store messages in our storage account
```
az storage container create -n messages --connection-string "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=rabeventstorage;AccountKey=T7b/yWPECiOmtaqg3ps5Osi0m3By7Cv/jtwB/ihi/SlVhw0xzpGSuV+srzpWPmAztmHespR+JsY9wIqskO6/Xw=="
```

### Evaluate Event Hub performance using the Azure portal.