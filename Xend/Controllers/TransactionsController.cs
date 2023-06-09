using System;
using Microsoft.AspNetCore.Mvc;
using Xend.MessageBrokers;
using Xend.Models;
using Xend.Repositories;
using Xend.Services;

namespace Xend.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<TransactionsController> _logger;
        private readonly IMessageBroker _messageBroker;
        private readonly ICryptoApiClient _cryptoApiClient;
        private IEventBus _eventBus;

        public TransactionsController(ITransactionRepository transactionRepository, IClientRepository clientRepository,
            ILogger<TransactionsController> logger, IMessageBroker messageBroker, ICryptoApiClient cryptoApiClient, IEventBus eventBus)
        {
            _transactionRepository = transactionRepository;
            _clientRepository = clientRepository;
            _logger = logger;
            _messageBroker = messageBroker;
            _cryptoApiClient = cryptoApiClient;
            _eventBus = eventBus;
        }

        [HttpGet("{clientId}")]
        public ActionResult<IEnumerable<Transaction>> GetTransactions(string clientId)
        {
            try
            {
                var transactions = _transactionRepository.GetTransactionsByClientId(clientId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve transactions.");
                return StatusCode(500, "An error occurred while retrieving transactions.");
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult> UpdateTransactions(UpdateTransactionsCommand command)
        {
            try
            {
                _logger.LogInformation("Received UpdateTransactions request for ClientId: {ClientId}, WalletAddress: {WalletAddress}, CurrencyType: {CurrencyType}",
                    command.ClientId, command.WalletAddress, command.CurrencyType);

                // Validate the command
                if (string.IsNullOrEmpty(command.ClientId) || string.IsNullOrEmpty(command.WalletAddress) || string.IsNullOrEmpty(command.CurrencyType))
                {
                    _logger.LogInformation("Invalid command. ClientId, WalletAddress, and CurrencyType are required.");
                    return BadRequest("Invalid command. ClientId, WalletAddress, and CurrencyType are required.");
                }

                // Check if the ClientId exists
                bool isClientIdValid = await _clientRepository.IsClientIdValidAsync(command.ClientId);
                if (!isClientIdValid)
                {
                    _logger.LogInformation("Invalid ClientId. The specified ClientId does not exist.");
                    return NotFound("Invalid ClientId. The specified ClientId does not exist.");
                }

                // Check for duplicate request
                bool isDuplicateRequest = await _transactionRepository.IsDuplicateRequestAsync(command.ClientId, command.WalletAddress, command.CurrencyType);
                if (isDuplicateRequest)
                {
                    _logger.LogInformation("Duplicate request");
                    return Conflict("Duplicate request"); // Return Conflict since it's a duplicate request
                }

                // Process the command and update transactions
                IEnumerable<Transaction> newTransactions = await _cryptoApiClient.GetTransactions(command.WalletAddress, command.CurrencyType);
                _transactionRepository.SaveTransactions(command.ClientId, newTransactions);

                var transactionReceivedEvent = new TransactionReceivedEvent();

                // Publish TransactionReceived event if new transactions were received
                if (newTransactions.Any())
                {
                    transactionReceivedEvent.ClientId = command.ClientId;
                    transactionReceivedEvent.Transactions = newTransactions;
                    _eventBus.Publish(transactionReceivedEvent);

                    _logger.LogInformation("TransactionReceived event published for ClientId: {ClientId}", command.ClientId);
                }

                if (transactionReceivedEvent != null)
                {
                    _logger.LogInformation("UpdateTransactions request completed successfully");
                    return Ok("Transaction updated");
                }
                else
                {
                    _logger.LogError("Failed to publish TransactionReceived event");
                    throw new Exception("Could not publish event");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update transactions.");
                return StatusCode(500, "An error occurred while updating transactions.");
            }
        }

    }

}

