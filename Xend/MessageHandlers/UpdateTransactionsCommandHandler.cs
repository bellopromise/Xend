using System;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using Xend.Models;
using Xend.Services;
using Xend.Repositories;
using RabbitMQ.Client;

namespace Xend.MessageHandlers
{
    public class UpdateTransactionsCommandHandler
    {
        private readonly ICryptoApiClient _cryptoApiClient;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IEventBus _eventBus;
        private readonly ILogger<UpdateTransactionsCommandHandler> _logger;
        private readonly IModel _channel;

        public UpdateTransactionsCommandHandler(
            ICryptoApiClient cryptoApiClient,
            ITransactionRepository transactionRepository,
            IEventBus eventBus,
            ILogger<UpdateTransactionsCommandHandler> logger,
            IModel channel)
        {
            _cryptoApiClient = cryptoApiClient;
            _transactionRepository = transactionRepository;
            _eventBus = eventBus;
            _logger = logger;
            _channel = channel;
        }

        public async Task HandleMessage(BasicDeliverEventArgs args)
        {
            var message = Encoding.UTF8.GetString(args.Body.ToArray());
            var command = JsonConvert.DeserializeObject<UpdateTransactionsCommand>(message);

            // Query the crypto API to get the transactions
            var transactions = await _cryptoApiClient.GetTransactions(command.WalletAddress, command.CurrencyType);

            // Check for duplicate transactions
            var newTransactions = FilterDuplicateTransactions(command.ClientId, transactions);

            // Publish "TransactionReceived" event for new transactions
            
                _eventBus.Publish(new TransactionReceivedEvent
                {
                    ClientId = command.ClientId,
                    Transactions = newTransactions
                });
            // Save transactions to the database
            _transactionRepository.SaveTransactions(command.ClientId, transactions);

            // Acknowledge the message
            _channel.BasicAck(args.DeliveryTag, false);
        }

        private IEnumerable<Transaction> FilterDuplicateTransactions(string clientId, IEnumerable<Transaction> transactions)
        {
            var existingTransactions = _transactionRepository.GetTransactionsByClientId(clientId);

            var newTransactions = transactions.Where(t => !existingTransactions.Any(et => et.Id == t.Id));

            return newTransactions;
        }
    }

}

