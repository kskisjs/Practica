using CRMApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMApp.Services
{
    public class ClientRepository : IRepository<ClientModel>
    {
        private CrmDataStore _store;

        public ClientRepository()
        {
            _store = JsonStorage.Load<CrmDataStore>(JsonStorage.CrmDataPath);
        }

        public Task<List<ClientModel>> GetAllAsync()
        {
            _store = JsonStorage.Load<CrmDataStore>(JsonStorage.CrmDataPath);
            return Task.FromResult(_store.Clients.ToList());
        }

        public Task AddAsync(ClientModel client)
        {
            client.Id = _store.NextClientId++;
            _store.Clients.Add(client);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(ClientModel client)
        {
            var existing = _store.Clients.FirstOrDefault(c => c.Id == client.Id);

            if (existing != null)
            {
                existing.FullName = client.FullName;
                existing.Phone = client.Phone;
                existing.Email = client.Email;
                existing.Company = client.Company;
                existing.ClientType = client.ClientType;
                existing.Notes = client.Notes;
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(ClientModel client)
        {
            _store.Clients.Remove(client);
            return Task.CompletedTask;
        }

        public Task SaveAsync()
        {
            JsonStorage.Save(JsonStorage.CrmDataPath, _store);
            return Task.CompletedTask;
        }
    }
}