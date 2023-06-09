using System;
using Xend.Models;
using Xend.Repositories;

namespace Xend.EventHandlers
{
    public class TransactionReceivedEventHandler : IEventHandler<TransactionReceivedEvent>
    {
        private readonly ILogger<TransactionReceivedEventHandler> _logger;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionReceivedEventHandler(ILogger<TransactionReceivedEventHandler> logger, ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
        }

        public async Task Handle(TransactionReceivedEvent @event)
        {
            try
            {
                // Process the received transaction event
                _logger.LogInformation($"Received transaction event for client with ID: {@event.ClientId}");

                // Save the transaction to the database
                _transactionRepository.SaveTransactions(@event.ClientId, @event.Transactions);

                // Additional processing or business logic can be added here

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing transaction event");
            }
        }
    }

}

