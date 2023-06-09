# Update Transactions

## Description

Hello, this  solution updates transaction by publishing them to Rabbitmq.  This was built using [Asp.Net Web Core](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-7.0) framework, [Rabbitmq](https://www.rabbitmq.com/) for event streaming, and [Microsoft SQL](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) for database storage.

## How to install

### Using Git (recommended)

```sh
$ git clone https://github.com/bellopromise/Xend # or clone your own fork
$ git checkout development
```
### Using manual download ZIP

1.  Download repository
2.  Uncompress to your desired directory


### Configuration

Set the connection string, and RabbitMQ configuration in the following files:

##### appSettings.json

-   `ConnectStrings.DefaultConnection`= **the Connection String of your database**
-   `RabbitMQ.HostName`= **the Host name of your RabbitMQ**
-   `RabbitMQ.Port`= **the port of your RabbitMQ**
-   `RabbitMQ.UserName`= **the username of your RabbitMQ**
-   `RabbitMQ.Password`= **the password of your RabbitMQ**
-   `RabbitMQ.ExchangeName`= **the exchange name of your RabbitMQ**

### Run the application using cli 
```bash
$ dotnet build
$ dotnet run
```

## Support
For more questions and clarifications, you can reach out to me here `bellopromise5322@gmail.com`


