using System;
using Newtonsoft.Json;
using Xend.Models;
using Xend.Services;
using System.Net.Http;
using System.Collections.Generic;

public class CryptoApiClient : ICryptoApiClient
{
    private readonly HttpClient _httpClient;

    public CryptoApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Transaction>> GetTransactions(string walletAddress, string currencyType)
    {
        string url = $"http://localhost:5000/crypto-api/transactions?walletAddress={walletAddress}&currencyType={currencyType}";
        // Make the API request to the crypto API
        HttpResponseMessage response = await _httpClient.GetAsync(url);

        // Handle the response and deserialize the transactions
        if (response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            var transactions = JsonConvert.DeserializeObject<IEnumerable<Transaction>>(responseBody);
            return transactions;
        }
        else
        { 
            // Handle the error response
            // You can throw an exception or return an empty collection based on your error handling strategy
            // For example:
            response.EnsureSuccessStatusCode(); // This will throw an exception if the response is not successful
            return Enumerable.Empty<Transaction>(); // This returns an empty collection
        }
    }
}
