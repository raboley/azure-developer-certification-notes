## Choose a messaging model in azure to loosely connect your services

Learning objectives

* Describe events and messages, and the challenges you can use them to solve in a distributed application.
* Identify scenarios in which Storage queue is the best messaging technology for an application.
* Identify scenarios in which Event Grid is the best messaging technology for an application.
* Identify scenarios in which Event Hubs is the best messaging technology for an application.
* Identify scenarios in which Service Bus is the best messaging technology for an application.

### Choosing Messages or events

| Messages | Events |
| -------- | ------ |
| Raw data | Light weight notification |
| Produced by a sender | Does not include the event, may reference where data is stored. | 
| Consumed by a reciever | Sender has no expectations of what the reciever will take. | 


* Questions to determine if you should use a message or an event.

1. Does the sending component expect it to be processed by the reciever in a particular way?
1. Does the communication include the data or payload?

If either are yes, then a message. If both are a no use an event.

Azure Connection technologies

| Technology | event or message | Type | Features |
| --- | --- | --- |  --- | 
| Azure Queue Storage | message | Queue | Rest API | 
| Azure Service Bus | message | Queue | Enterprise | 
| Azure Event Grid | Event | 

### Azure Service bus and Azure Storage Queues

Azure Service Bus Topics are like queues but can have multiple subscribes. When something is sent to a topic instead of a queue multiple components can be triggered to do their work. Internally this works by using a queue, where the message is copied and dropped onto a queue for each subscriber meaning the message copy will stay around and be processed by each subscription even if the component processing that subscription is too busy to keep up.

Delivery Guarantees: 

* At-least-once Delivery: A message will go to at least one reciever. If there are two instances of a web app, and one takes too long it could send the message to the other instance as well. This means there could be multiple deliveries of the same message.
* At-Most-Once Delivery: Messages aren't guaranteed to be delivered, but they will not be delivered twice, kind of like duplication protection.
* First-In-Frist-Out (FIFO): Queue is processed in the exact same order in which they were added.

Transactions: Queues can have transactions, so tightly grouped messages like an order fulfillment, credit card payment and order confirmation can succeed only if all succeed using a transaction just like a database.

Choose Service Bus Topics if
* you need multiple receivers to handle each message

Choose Service Bus queues if:

* You need an At-Most-Once delivery guarantee.
* You need a FIFO guarantee.
* You need to group messages into transactions.
* You want to receive messages without polling the queue.
* You need to provide a role-based access model to the queues.
* You need to handle messages larger than 64 KB but less than 256 KB.
* Your queue size will not grow larger than 80 GB.
* You would like to be able to publish and consume batches of messages.

Choose Queue storage if:

* You need an audit trail of all messages that pass through the queue.
* You expect the queue to exceed 80 GB in size.
* You want to track progress for processing a message inside of the queue.

### Event Grid

Event grids have topics from publishers that route to subscriptions which are picked up by subscribers. It is designed for a one event at a time style of delivery. An event can be up to `64KB` in size.

It is simple to setup and can use a lot of recievers and send to lots of subscribers. Use Event grid when:

* Simplicity: It is straightforward to connect sources to subscribers in Event Grid.
* Advanced filtering: Subscriptions have close control over the events they receive from a topic.
* Fan-out: You can subscribe to an unlimited number of endpoints to the same events and topics.
* Reliability: Event Grid retries event delivery for up to 24 hours for each subscription.
* Pay-per-event: Pay only for the number of events that you transmit.

### Event Hub

Pub-Sub pattern with high resiliancy and throughput. It can handle processing batches of data, or stream realtime. Event Grid is more of an event router, where event hub has more features.

#### Partitions

Messages are stored in partitions as they come in, and subscribers can catch up if they fall behind by accessing the buffer of messages which is by default 24 hours.

#### Capture

Event hub can send events to Azure Data Lake or Azure Blob storage for cheap persistent storage.

#### Authentication

All pubs are authenticated with a token.

Choose Event Hubs if:
* You need to support authenticating a large number of publishers.
* You need to save a stream of events to Data Lake or Blob storage.
* You need aggregation or analytics on your event stream.
* You need reliable messaging or resiliency.