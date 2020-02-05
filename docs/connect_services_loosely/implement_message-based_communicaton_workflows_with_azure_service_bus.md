# Implementing Azure Service Bus

## Learning Goals

* Choose whether to use Service Bus queues, topics, or relays to communicate in a distributed application
* Configure an Azure Service Bus namespace in an Azure subscription
* Create a Service Bus topic and use it to send and receive messages
* Create a Service Bus queue and use it to send and receive messages

### Choose whether to use Service Bus queues, topics, or relays to communicate in a distributed application

#### Queues

Queue is a temporary storage location for messages where a sender adds messages to the queue, and recievers pick up messages at the front. Normally just one sender and reciever.

Use Queues when there is one reciever per sender and you want to be able to handle high load. More specific reasons.

* You need an at-most-once delivery guarantee
* You need a FIFO guarantee
* You need to group messages into transactions
* You want to receive messages without polling the queue
* You need to provide role-based access to the queues
* You need to handle messages larger than 64 KB but smaller than 256 KB
* Your queue size will not grow larger than 80 GB
* You would like to be able to publish and consume batches of messages

Example:

A pizza company wants to send sales to a particular web service based on region.

#### Topics

Topics are are like queues but can handle multiple subscribers. This is best used if you plan to fan out messages. It is not included in the free tier.

![topics_fan_messages](../../topics_fan_messages.png)

Use Topics if there is a need to fan out a sender's message to multiple recievers.

Example: 

A pizza company wants to send sales performance data to all web services within all regions.

#### Relay

Relay is for synchronous communication between two devices acting as if they were on the same network. It is not loosely coupled if you use this method.

Try not to use relays, but if there is a need for bi-directional synchronous communication they are a solution.


### Configure an Azure Service Bus namespace in an Azure subscription

Namespace is a container with a fully qualified domain name. A topic/queue/relay starts by having a namespace. There are access keys to authenticate all senders and recievers.

### Create a Service Bus topic and use it to send and receive messages

Easily done in the portal searching for azure service bus. Create a namespace, and then topics, queues and relays can be created.

One trick is to use this command to get the connection string.

``` bash
az servicebus namespace authorization-rule keys list \
    --resource-group <resource-group> \
    --name RootManageSharedAccessKey \
    --query primaryConnectionString \
    --output tsv \
    --namespace-name <namespace-name>
```

### Create a Service Bus queue and use it to send and receive messages