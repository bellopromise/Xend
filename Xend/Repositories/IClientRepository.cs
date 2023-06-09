using System;
using Xend.Models;

namespace Xend.Repositories
{
    public interface IClientRepository
    {
        Task<bool> IsClientIdValidAsync(string clientId);
        Task<Client> GetClientByClientIdAsync(string clientId);
    }

}

