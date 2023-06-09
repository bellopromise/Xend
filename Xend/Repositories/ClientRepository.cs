using System;
using Microsoft.EntityFrameworkCore;
using Xend.Data;
using Xend.Models;

namespace Xend.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ClientRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsClientIdValidAsync(string clientId)
        {
            // Check if the client with the specified clientId exists
            return await _dbContext.Clients.AnyAsync(c => c.ClientId == clientId);
        }

        public async Task<Client> GetClientByClientIdAsync(string clientId)
        {
            // Retrieve the client with the specified clientId
            return await _dbContext.Clients.FirstOrDefaultAsync(c => c.ClientId == clientId);
        }
    }

}

