using System.Collections.Generic;
using System.Threading.Tasks;
using CRMApp.Models;

namespace CRMApp.Services
{
    public interface IClientService
    {
        Task<List<ClientModel>> GetAllClientsAsync();
        Task<ClientModel?> GetClientByIdAsync(int id);
        Task AddClientAsync(ClientModel client);
        Task UpdateClientAsync(ClientModel client);
        Task DeleteClientAsync(int id);
    }
}
